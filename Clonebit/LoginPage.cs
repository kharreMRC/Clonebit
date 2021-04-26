using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using Gtk;
using MySqlConnector;

namespace Clonebit
{
    [System.ComponentModel.ToolboxItem(true)]
    public partial class LoginPage : Bin
    {
        public LoginPage()
        {
            Build();
        }

        protected void OnLoginButtonClicked(object sender, EventArgs e)
        {
            try
            {
                // Attempt of login to the database
                MainWindow.connector = new MySqlConnection("Server=127.0.0.1; User ID=" +
                                                usernameEntry.Text +
                                                "; Password=" +
                                                passwordEntry.Text +
                                                "; Database=" +
                                                databaseEntry.Text);

                // Login in the database
                MainWindow.connector.Open();

                MainWindow.ExecuteSQLCommand($"INSERT INTO log VALUES (0, now(), 'Successful login', '{usernameEntry.Text}');");

                Clonebit.Settings.Serials = new List<string>();
                using (var cmd = new MySqlCommand("SELECT usb_serial FROM usb_storage;", MainWindow.connector))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Clonebit.Settings.Serials.Add(reader.GetString(0));
                    }
                }
                SettingsWindow.logStatus = true;

                MainWindow.vBox.Remove(this);
                MainWindow.vBox.PackEnd(new HomePage(), true, true, 0);
                MainWindow.vBox.ShowAll();

                // Ping stations, add and show station pages
                MainWindow.pingThread = new Thread(new ThreadStart(MainWindow.GetStationStatus));
                MainWindow.pingThread.Start();

                MainWindow.logoutItem.Sensitive = true;
            }
            catch (MySqlException)
            {
                MainWindow.connector = new MySqlConnection("Server=127.0.0.1; User ID=user; Password=user; Database=duplication");
                MainWindow.connector.Open();
                MainWindow.ExecuteSQLCommand($"INSERT INTO log VALUES" +
                	                          "(0, now(), 'Fail login : " +
                	                          "user=\"{usernameEntry.Text}\"," +
                	                          "password=\"{passwordEntry.Text}\"," +
                	                          "database=\"{databaseEntry.Text}\"'," +
                	                          "'log_user');");
                MainWindow.connector.Close();

                using (var messageDialog = new MessageDialog(null,
                                                             DialogFlags.DestroyWithParent,
                                                             MessageType.Error,
                                                             ButtonsType.Ok,
                                                             "Les identifiants sont incorrects."))
                {
                    messageDialog.Title = "Erreur de connexion";
                    messageDialog.Run();
                    messageDialog.Destroy();
                }
            }

            usernameEntry.Text = "";
            passwordEntry.Text = "";
            databaseEntry.Text = "";
        }

        public int GetUSBCount()
        {
            var count = 0;
            using (var cmd = new MySqlCommand("SELECT count(*) FROM usb_storage;", MainWindow.connector))
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    count = reader.GetByte(0);
                }
            }
            return count;
        }

        private string GetStringFingerprint(string text)
        {
            using (SHA512 sha256 = SHA512.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.ASCII.GetBytes(text));
                StringBuilder stringBuilder = new StringBuilder();
                for (var i = 0; i < bytes.Length; i++)
                {
                    stringBuilder.Append(bytes[i].ToString("x2"));
                }
                return stringBuilder.ToString();
            }
        }

        //private bool GetFingerprintComparison(string firstFprint, string secondFprint)
        //{
        //    if (firstFprint.Equals(secondFprint))
        //    {
        //        return true;
        //    }
        //    return false;
        //}
    }
}
