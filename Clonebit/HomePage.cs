using System;
using System.IO;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;
using Gtk;
using Renci.SshNet;

namespace Clonebit
{
    [System.ComponentModel.ToolboxItem(true)]
    public partial class HomePage : Bin
    {
        public static ListStore stationStore;

        private FileType fileType;

        private FileChooserAction fileChooserAction;

        private StringBuilder command;

        private string fileFingerprint;
        private string fullFileName;
        private string shortFileName;
        private string fileDestination;
        private long fileSize;

        public HomePage()
        {
            Build();

            // List selected stations
            TreeViewColumn stationColumn = new TreeViewColumn
            {
                Title = "N° station"
            };
            CellRendererText stationCell = new CellRendererText();
            stationColumn.PackStart(stationCell, true);

            TreeViewColumn addressColumn = new TreeViewColumn
            {
                Title = "Adresse IPv4"
            };
            CellRendererText addressCell = new CellRendererText();
            addressColumn.PackStart(addressCell, true);

            TreeViewColumn usbColumn = new TreeViewColumn
            {
                Title = "USB branché"
            };
            CellRendererText usbCell = new CellRendererText();
            usbColumn.PackStart(usbCell, true);

            stationTreeView.AppendColumn(stationColumn);
            stationTreeView.AppendColumn(addressColumn);
            stationTreeView.AppendColumn(usbColumn);

            stationColumn.AddAttribute(stationCell, "text", 0);
            addressColumn.AddAttribute(addressCell, "text", 1);
            usbColumn.AddAttribute(usbCell, "text", 2);

            stationStore = new ListStore(typeof(string), typeof(string), typeof(string) );
            stationTreeView.Model = stationStore;
        }

        protected void OnOpenButtonClicked(object sender, EventArgs e)
        {
            // Open a file chooser window
            FileChooserDialog fcd = new FileChooserDialog(
                "Sélection du fichier à dupliquer",
                null, fileChooserAction,
                "Annuler", ResponseType.Cancel,
                "Ouvrir", ResponseType.Accept)
            {
                DefaultResponse = ResponseType.Ok,
                SelectMultiple = false
            };

            if (fcd.Run() == (int)ResponseType.Accept)
            {
                // If the user want to duplicate a repository...
                if (fileChooserAction.Equals(FileChooserAction.SelectFolder))
                {
                    DirectoryInfo directoryInfo = new DirectoryInfo(fcd.Filename);

                    // Set folder values
                    fileFingerprint = "";
                    fullFileName = directoryInfo.FullName + "/";
                    shortFileName = directoryInfo.Name;
                    fileSize = GetRepositorySize(directoryInfo);

                    // Set UI folder information
                    filenameLabel.Text = SetItalic(shortFileName + "/");
                    filenameInfoLabel.Text = SetItalic(directoryInfo.FullName + "/");
                    sizeInfoLabel.Text = SetItalic(GetRepositorySize(directoryInfo) + " octets");
                    parentRepositoryInfoLabel.Text = SetItalic(directoryInfo.Parent + "/");
                    lastAccessDateInfoLabel.Text = SetItalic(directoryInfo.LastAccessTime.ToString());
                    lastWriteDateInfoLabel.Text = SetItalic(directoryInfo.LastWriteTime.ToString());
                    fingerprintInfoLabel.Text = SetItalic("Empreinte MD5 indisponible");
                }
                // ... or a file
                else
                {
                    FileInfo fileInfo = new FileInfo(fcd.Filename);

                    // Set file values
                    fileFingerprint = GetFileFingerprint(fileInfo.FullName);
                    fullFileName = fileInfo.FullName;
                    shortFileName = GetFileName(fullFileName);
                    fileSize = fileInfo.Length;

                    // Set UI file information
                    filenameLabel.Text = SetItalic(shortFileName);
                    filenameInfoLabel.Text = SetItalic(fileInfo.FullName);
                    sizeInfoLabel.Text = SetItalic(fileInfo.Length.ToString() + " octets");
                    parentRepositoryInfoLabel.Text = SetItalic(fileInfo.DirectoryName + "/");
                    lastAccessDateInfoLabel.Text = SetItalic(fileInfo.LastAccessTime.ToString());
                    lastWriteDateInfoLabel.Text = SetItalic(fileInfo.LastWriteTime.ToString());
                    fingerprintInfoLabel.Text = SetItalic(fileFingerprint);
                }

                // Apply tags
                filenameLabel.UseMarkup = true;
                filenameInfoLabel.UseMarkup = true;
                sizeInfoLabel.UseMarkup = true;
                parentRepositoryInfoLabel.UseMarkup = true;
                lastAccessDateInfoLabel.UseMarkup = true;
                lastWriteDateInfoLabel.UseMarkup = true;
                fingerprintInfoLabel.UseMarkup = true;

                // Unsensitive the other frames
                filenameLabel.Sensitive = true;
                fileFrame.Sensitive = true;
                duplicationFrame.Sensitive = true;
            }
            fcd.Destroy();
        }

        protected void OnTypeFileComboBoxChanged(object sender, EventArgs e)
        {
            switch (typeFileComboBox.ActiveText)
            {
                case "Fichier":
                    fileType = FileType.File;
                    fileChooserAction = FileChooserAction.Open;
                    fileDestination = "TO_CP";
                    openButton.Sensitive = true;
                    break;
                case "Répertoire":
                    fileType = FileType.Folder;
                    fileChooserAction = FileChooserAction.SelectFolder;
                    fileDestination = "TO_CP";
                    openButton.Sensitive = true;
                    break;
                default:
                    // By default, everything is unsensitive
                    openButton.Sensitive = false;
                    duplicationFrame.Sensitive = false;
                    fileFrame.Sensitive = false;
                    break;
            }
        }

        protected void OnDuplicateButtonClicked(object sender, EventArgs e)
        {
            // /root/start_multicast.sh [filepath]
            command = new StringBuilder("/root/start_multicast.sh /nfs/shared_folder/TO_CP/");

            // Filter file name
            string message;
            if (shortFileName.Contains(" ") || shortFileName.Contains("'"))
            {
                message = "Le nom du fichier spécifié contient des caractères interdits. " +
                          "En conséquence, le nom du fichier va être automatiquement modifié au lancement de la duplication.\n" +
                          "La duplication est sur le point de débuter. Continuer ?";
                WriteLogs("Clonebit", "WARNING", $"Filename : {fullFileName}");
            }
            else
            {
                message = "La duplication est sur le point de débuter. Continuer ?";
                WriteLogs("Clonebit", "OK", $"Filename : {fullFileName}");
            }
            var duplicationDialog = new MessageDialog(null,
                                                      DialogFlags.DestroyWithParent,
                                                      MessageType.Warning,
                                                      ButtonsType.YesNo,
                                                      message)
            {
                Title = "Confirmation de la duplication"
            };
            if ((ResponseType)duplicationDialog.Run() == ResponseType.Yes)
            {
                Sensitive = false;
                duplicationDialog.Destroy();
                try
                {
                    switch (fileType)
                    {
                        case FileType.File:
                            // Copy file to NFS
                            command.Append(shortFileName);
                            File.Copy(fullFileName, $"{MainWindow.settings.NFSPath}{fileDestination}/{PurifyFileName(shortFileName)}", true);
                            logTextView.Buffer.Text += $"[{DateTime.Now.ToLongTimeString()}] Clonebit > [OK] Copie du fichier sur le NFS\n";
                            break;
                        case FileType.Folder:
                            CopyFolder(fullFileName, $"{MainWindow.settings.NFSPath}{fileDestination}/{PurifyFileName(shortFileName)}");
                            logTextView.Buffer.Text += $"[{DateTime.Now.ToLongTimeString()}] Clonebit > [OK] Copie du répertoire sur le NFS\n";
                            break;
                    }
                }
                catch (Exception)
                {
                    WriteLogs("Clonebit", "FAIL", "Cannot copy the file to the NFS");
                    using (var messageDialog = new MessageDialog(null,
                                                                 DialogFlags.DestroyWithParent,
                                                                 MessageType.Error,
                                                                 ButtonsType.Ok,
                                                                 "La copie du fichier sur le NFS a échoué."))
                    {
                        messageDialog.Title = "Erreur copie";
                        messageDialog.Run();
                        messageDialog.Destroy();
                    }
                }

                // Write event in database
                try
                {
                    // THERE IS A PROBLEM HERE ON PRODUCTION ENVIRONMENT
                    MainWindow.ExecuteSQLCommand($"INSERT INTO file VALUES (0, '{fileFingerprint}', '{PurifyFileName(fullFileName)}', {fileSize});");
                    WriteLogs("Clonebit", "OK", "Logs successfully inserted in database");
                }
                catch (Exception)
                {
                    WriteLogs("Clonebit", "FAIL", "Cannot insert logs in database");
                    using (var messageDialog = new MessageDialog(null,
                                                                 DialogFlags.DestroyWithParent,
                                                                 MessageType.Error,
                                                                 ButtonsType.Ok,
                                                                 "L'écriture des évènements en base de données a échoué."))
                    {
                        messageDialog.Title = "Erreur base de données";
                        messageDialog.Run();
                        messageDialog.Destroy();
                    }
                }

                // Run duplication via SSH
                var connectionInfo = new ConnectionInfo("192.168.1.1", 22, "root", new AuthenticationMethod[]
                {
                            new PrivateKeyAuthenticationMethod("root", new PrivateKeyFile[] { new PrivateKeyFile(MainWindow.settings.PrivateKeyPath, "") })
                });
                using (var client = new SshClient(connectionInfo))
                {
                    try
                    {
                        client.Connect();
                        WriteLogs("Clonebit", "OK", "Successful login to the server");
                        Console.WriteLine("\n[SSH] Successful login to the server");
                        var SSHCommand = client.CreateCommand(command.ToString());

                        // Get continuously logs from the server after running the command
                        var exitCode = SSHCommand.BeginExecute();
                        using (var streamReader = new StreamReader(SSHCommand.OutputStream, Encoding.UTF8, true, 1024, true))
                        {
                            while (!exitCode.IsCompleted || !streamReader.EndOfStream)
                            {
                                string currentLine = streamReader.ReadLine();
                                if (currentLine != null)
                                {
                                    WriteLogs("Server", currentLine);
                                }
                            }
                        }
                        SSHCommand.EndExecute(exitCode);
                        client.Disconnect();
                        WriteLogs("Clonebit", "OK", "Successful logout from the server");



                        using (var messageDialog = new MessageDialog(null,
                                                                 DialogFlags.DestroyWithParent,
                                                                 MessageType.Info,
                                                                 ButtonsType.Ok,
                                                                 "La duplication s'est terminée avec succès."))
                        {
                            messageDialog.Title = "Succès de la duplication";
                            messageDialog.Run();
                            messageDialog.Destroy();
                        }
                    }
                    catch (Exception)
                    {
                        WriteLogs("Clonebit", "FAIL", "Cannot connect to the SSH server");
                        WriteLogs("Clonebit", "FAIL", "Duplication");
                        using (var messageDialog = new MessageDialog(null,
                                                                 DialogFlags.DestroyWithParent,
                                                                 MessageType.Error,
                                                                 ButtonsType.Ok,
                                                                 "La connexion SSH à l'ordonnanceur a échoué."))
                        {
                            messageDialog.Title = "Erreur connexion SSH";
                            messageDialog.Run();
                            messageDialog.Destroy();
                        }
                    }
                }
                command.Clear();
                Sensitive = true;
            }
            else
            {
                duplicationDialog.Destroy();
            }
        }

        protected void OnClearButtonClicked(object sender, EventArgs e)
        {
            logTextView.Buffer.Clear();
        }

        protected void OnExportButtonClicked(object sender, EventArgs e)
        {
            FileChooserDialog fcd = new FileChooserDialog(
                "Enregistrer la journalisation",
                null, FileChooserAction.Save,
                "Annuler", ResponseType.Cancel,
                "Enregistrer", ResponseType.Accept)
            {
                DefaultResponse = ResponseType.Ok,
                SelectMultiple = false
            };
            if (fcd.Run() == (int)ResponseType.Accept)
            {
                File.AppendAllText(fcd.Filename, logTextView.Buffer.Text);
            }
            fcd.Destroy();
        }

        private void CopyFolder(string source, string target)
        {
            DirectoryInfo sourceInfo = new DirectoryInfo(source);
            DirectoryInfo targetInfo = new DirectoryInfo(target);
            targetInfo.Create();
            foreach (var directory in sourceInfo.GetDirectories())
            {
                CopyFolder(directory.FullName, targetInfo.CreateSubdirectory(directory.Name).FullName);
            }
            foreach (var file in sourceInfo.GetFiles())
            {
                file.CopyTo($"{targetInfo.FullName}/{file.Name}", true);
            }
        }

        private void WriteLogs(string from, string status, string text)
        {
            Console.WriteLine($"[{DateTime.Now.ToLongTimeString()}] {from} > [{status}] {text}\n");
            logTextView.Buffer.Text += $"[{DateTime.Now.ToLongTimeString()}] {from} > [{status}] {text}\n";
        }

        private void WriteLogs(string from, string text)
        {
            Console.WriteLine($"[{DateTime.Now.ToLongTimeString()}] {from} > {text}\n");
            logTextView.Buffer.Text += $"[{DateTime.Now.ToLongTimeString()}] {from} > {text}\n";
        }

        private string PingStation(string address)
        {
            Ping pingRequest = new Ping();
            PingReply pingReply = pingRequest.Send(address);
            return pingReply.Status.ToString();
        }

        private long GetRepositorySize(DirectoryInfo directoryInfo)
        {
            long sum = 0;
            foreach (var directory in directoryInfo.GetDirectories())
            {
                sum += GetRepositorySize(directory);
            }
            foreach (var file in directoryInfo.GetFiles())
            {
                if (!file.Attributes.Equals(FileAttributes.Directory))
                {
                    sum += file.Length;
                }
            }
            return sum;
        }

        private string GetFileFingerprint(string path)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] bytes = md5.ComputeHash(File.ReadAllBytes(path));
                StringBuilder stringBuilder = new StringBuilder();
                for (var i = 0; i < bytes.Length; i++)
                {
                    stringBuilder.Append(bytes[i].ToString("x2"));
                }
                return stringBuilder.ToString();
            }
        }

        private string GetFileName(string path)
        {
            return path.Split('/')[path.Split('/').Length - 1];
        }

        private string SetItalic(string text)
        {
            return "<i>" + text + "</i>";
        }

        private string PurifyFileName(string filename)
        {
            return filename.Replace('\'', ' ').Replace(' ', '_');
        }

        // Unused
        //private string GetParentRepository(string filename)
        //{
        //    StringBuilder stringBuilder = new StringBuilder();
        //    for (var i = 0; i < filename.Split('/').Length - 2; i++)
        //    {
        //        stringBuilder.Append(fullFileName.Split('/')[i] + "/");
        //    }
        //    return stringBuilder.ToString();
        //}
    }
}
