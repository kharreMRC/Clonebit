using System.IO;
using Newtonsoft.Json;

namespace Clonebit
{
    public partial class SettingsWindow : Gtk.Window
    {
        public static bool logStatus;

        public SettingsWindow() :
                base(Gtk.WindowType.Toplevel)
        {
            Build();
            if (!logStatus)
            {
                serialFrame.Sensitive = false;
            }
            else
            {
                for (int i = 0; i < Clonebit.Settings.Serials.Count; i++)
                {
                    serialComboBox.AppendText($"Stockage {i + 1}");
                }
                stationFrame.Sensitive = false;
                otherSettingsFrame.Sensitive = false;
            }
            for (int i = 0; i < MainWindow.settings.Addresses.Count; i++)
            {
                stationComboBox.AppendText($"Station {i + 1}");
            }
            privateKeyEntry.Text = MainWindow.settings.PrivateKeyPath;
            NFSEntry.Text = MainWindow.settings.NFSPath;
            logsEntry.Text = MainWindow.settings.LogsPath;
        }

        protected void OnStationComboBoxChanged(object sender, System.EventArgs e)
        {
            addressEntry.Text = MainWindow.settings.Addresses[int.Parse(stationComboBox.ActiveText.Split()[1]) - 1];
        }

        protected void OnAddButtonClicked(object sender, System.EventArgs e)
        {
            stationComboBox.AppendText($"Station {MainWindow.settings.Addresses.Count + 1}");
            MainWindow.settings.Addresses.Add("192.168.1." + (10 + MainWindow.settings.Addresses.Count + 1).ToString());
        }

        protected void OnModifyButtonClicked(object sender, System.EventArgs e)
        {
            MainWindow.settings.Addresses[int.Parse(stationComboBox.ActiveText.Split()[1]) - 1] = addressEntry.Text;
        }

        protected void OnSerialComboBoxChanged(object sender, System.EventArgs e)
        {
            serialEntry.Text = Clonebit.Settings.Serials[int.Parse(serialComboBox.ActiveText.Split()[1]) - 1];
        }

        protected void OnSerialButtonClicked(object sender, System.EventArgs e)
        {
            serialComboBox.AppendText($"Stockage {Clonebit.Settings.Serials.Count + 1}");
            Clonebit.Settings.Serials.Add("");
            MainWindow.ExecuteSQLCommand($"INSERT INTO usb_storage VALUES ({Clonebit.Settings.Serials.Count - 1}, '', 0);");
        }

        protected void OnModifySerialButtonClicked(object sender, System.EventArgs e)
        {
            var stationNo = int.Parse(serialComboBox.ActiveText.Split()[1]) - 1;
            Clonebit.Settings.Serials[int.Parse(serialComboBox.ActiveText.Split()[1]) - 1] = serialEntry.Text;
            MainWindow.ExecuteSQLCommand($"UPDATE usb_storage SET usb_serial='{serialEntry.Text}' WHERE usb_id={stationNo};");
        }

        protected void OnApplyButtonClicked(object sender, System.EventArgs e)
        {
            MainWindow.settings.PrivateKeyPath = privateKeyEntry.Text;
            MainWindow.settings.NFSPath = NFSEntry.Text;
            MainWindow.settings.LogsPath = logsEntry.Text;

            string content = JsonConvert.SerializeObject(MainWindow.settings);
            File.WriteAllText("settings.json", content);
        }
    }
}
