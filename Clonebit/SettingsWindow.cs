namespace Clonebit
{
    public partial class SettingsWindow : Gtk.Window
    {
        private string[] addresses;

        public SettingsWindow(string[] addresses) :
                base(Gtk.WindowType.Toplevel)
        {
            Build();
            this.addresses = addresses;
            for (int i = 0; i < addresses.Length; i++)
            {
                stationComboBox.AppendText($"Station {i + 1}");
            }
        }

        protected void OnStationComboBoxChanged(object sender, System.EventArgs e)
        {
            switch (stationComboBox.ActiveText)
            {
                case "Station 1":
                    addressEntry.Text = addresses[0];
                    break;
                case "Station 2":
                    addressEntry.Text = addresses[1];
                    break;
                case "Station 3":
                    addressEntry.Text = addresses[2];
                    break;
                case "Station 4":
                    addressEntry.Text = addresses[3];
                    break;
                case "Station 5":
                    addressEntry.Text = addresses[4];
                    break;
                case "Station 6":
                    addressEntry.Text = addresses[5];
                    break;
            }
        }

        protected void OnApplyButtonClicked(object sender, System.EventArgs e)
        {
            MainWindow.privateKeyPath = privateKeyEntry.Text;
            MainWindow.NFSPath = NFSEntry.Text;
        }
    }
}
