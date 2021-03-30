using System;
using System.IO;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Gtk;
using MySqlConnector;
using Renci.SshNet;

namespace Clonebit
{
    [System.ComponentModel.ToolboxItem(true)]
    public partial class HomePage : Bin
    {
        // TODO
        // Inscrire log dans BDD à partir de /media/usblog.json

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
                    fingerprintInfoLabel.Text = SetItalic("Empreinte SHA256 indisponible");
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
                case "Fichier (support d'amorçage)":
                    fileType = FileType.DDFile;
                    fileChooserAction = FileChooserAction.Open;
                    fileDestination = "TO_DD";
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

        protected async void OnDuplicateButtonClicked(object sender, EventArgs e)
        {
            // Blank space is important because arguments will be added
            command = new StringBuilder("/root/bin/start-copy.sh ");

            // Filter file name
            string message;
            if (shortFileName.Contains(" ") || shortFileName.Contains("'"))
            {
                message = "Le nom du fichier spécifié contient des caractères interdits. " +
                          "En conséquence, le nom du fichier va être automatiquement modifié au lancement de la duplication.\n" +
                          "La duplication est sur le point de débuter. Continuer ?";
            }
            else
            {
                message = "La duplication est sur le point de débuter. Continuer ?";
            }

            using (var duplicationDialog = new MessageDialog(null,
                                                     DialogFlags.DestroyWithParent,
                                                     MessageType.Warning,
                                                     ButtonsType.YesNo,
                                                     message))
            {
                duplicationDialog.Title = "Confirmation de la duplication";
                if ((ResponseType)duplicationDialog.Run() == ResponseType.Yes)
                {
                    duplicationDialog.Destroy();
                    try
                    {
                        switch (fileType)
                        {
                            case FileType.File:
                                // Build bash command
                                command.Append("cp ");
                                // Copy file to NFS
                                File.Copy(fullFileName, $"{MainWindow.NFSPath}{fileDestination}/{PurifyFileName(shortFileName)}", true);
                                break;
                            case FileType.Folder:
                                command.Append("cp ");
                                CopyFolder(fullFileName, $"{MainWindow.NFSPath}{fileDestination}/{PurifyFileName(shortFileName)}");
                                break;
                            case FileType.DDFile:
                                command.Append("dd ");
                                File.Copy(fullFileName, $"{MainWindow.NFSPath}{fileDestination}/{PurifyFileName(shortFileName)}", true);
                                break;
                        }
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("[SERVER] Copy error");
                    }

                    // Write event in database
                    try
                    {
                        // THERE IS A PROBLEM HERE ON PRODUCTION ENVIRONMENT
                        await AsyncExecuteSQLCommand($"INSERT INTO file VALUES (0, '{fileFingerprint}', '{PurifyFileName(fullFileName)}', {fileSize});");
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("[BDD] Logging error");
                    }

                    // Finish building bash command
                    for (var i = 0; i < MainWindow.addresses.Length; i++)
                    {
                        var stationNo = i + 11;
                        if (PingStation(new StringBuilder($"192.168.1.{stationNo.ToString()}").ToString()).Equals("Success"))
                        {
                            Console.WriteLine($"[PING] {MainWindow.addresses[i]} success");
                            command.Append($"{stationNo.ToString()} ");
                        }
                        else
                        {
                            Console.WriteLine($"[PING] {MainWindow.addresses[i]} fail");
                        }
                    }

                    // Run duplication via SSH
                    var connectionInfo = new ConnectionInfo("192.168.1.1", 22, "root", new AuthenticationMethod[]
                    {
                            new PrivateKeyAuthenticationMethod("root", new PrivateKeyFile[] { new PrivateKeyFile(MainWindow.privateKeyPath, "") })
                    });
                    using (var client = new SshClient(connectionInfo))
                    {
                        try
                        {
                            client.Connect();
                            Console.WriteLine($"\n[DEBUG] {command}");
                            Console.WriteLine("\n========== SERVER ==========\n" +
                                             $"{client.CreateCommand(command.ToString()).Execute()}" +
                                              "============================");
                            client.Disconnect();
                            Console.WriteLine("\n[SERVER] Disconnected");
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("\n[SERVER] SSH error");
                        }
                    }
                    command.Clear();
                }
                else
                {
                    duplicationDialog.Destroy();
                }
            }
        }

        private async Task AsyncExecuteSQLCommand(string cmd)
        {
            var sqlCommand = new MySqlCommand(cmd, MainWindow.connector);
            await sqlCommand.ExecuteNonQueryAsync();
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

        private string PingStation(string address)
        {
            Ping pingRequest = new Ping();
            PingReply pingReply = pingRequest.Send(address, 2);
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
        private string GetParentRepository(string filename)
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (var i = 0; i < filename.Split('/').Length - 2; i++)
            {
                stringBuilder.Append(fullFileName.Split('/')[i] + "/");
            }
            return stringBuilder.ToString();
        }
    }
}
