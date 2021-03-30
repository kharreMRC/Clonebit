﻿using System;
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

                // Remove login page
                MainWindow.mainNotebook.RemovePage(0);

                // Add home page
                MainWindow.mainNotebook.AppendPage(new HomePage(), new Label("Accueil"));

                // Show all pages and then hide login page
                MainWindow.mainNotebook.ShowAll();

                // Ping stations, add and show station pages
                MainWindow.pingThread = new Thread(new ThreadStart(MainWindow.GetStationStatus));
                MainWindow.pingThread.Start();

                MainWindow.logoutItem.Sensitive = true;
                //MainWindow.refreshItem.Sensitive = true;
            }
            catch (MySqlException)
            {
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

        //// Unused
        //private string GetStringFingerprint(string text)
        //{
        //    using (SHA256 sha256 = SHA256.Create())
        //    {
        //        byte[] bytes = sha256.ComputeHash(Encoding.ASCII.GetBytes(text));
        //        StringBuilder stringBuilder = new StringBuilder();
        //        for (var i = 0; i < bytes.Length; i++)
        //        {
        //            stringBuilder.Append(bytes[i].ToString("x2"));
        //        }
        //        return stringBuilder.ToString();
        //    }
        //}

        //// Unused
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
