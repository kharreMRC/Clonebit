using System.IO;
using Newtonsoft.Json;

namespace Clonebit
{
    public partial class SettingsWindow : Gtk.Window
    {
        Settings settings;

        public SettingsWindow() :
                base(Gtk.WindowType.Toplevel)
        {
            Build();

            string content = File.ReadAllText("settings.json");
            settings = JsonConvert.DeserializeObject<Settings>(content);
            
            for (int i = 0; i < settings.Addresses.Length; i++)
            {
                stationComboBox.AppendText($"Station {i + 1}");
            }
            privateKeyEntry.Text = settings.PrivateKeyPath;
            NFSEntry.Text = settings.NFSPath;
        }

        protected void OnStationComboBoxChanged(object sender, System.EventArgs e)
        {
            addressEntry.Text = settings.Addresses[int.Parse(stationComboBox.ActiveText.Split()[1]) - 1];
        }

        protected void OnModifyButtonClicked(object sender, System.EventArgs e)
        {
            settings.Addresses[int.Parse(stationComboBox.ActiveText.Split()[1]) - 1] = addressEntry.Text;
        }

        protected void OnApplyButtonClicked(object sender, System.EventArgs e)
        {
            MainWindow.privateKeyPath = privateKeyEntry.Text;
            MainWindow.NFSPath = NFSEntry.Text;
            settings.PrivateKeyPath = privateKeyEntry.Text;
            settings.NFSPath = NFSEntry.Text;

            string content = JsonConvert.SerializeObject(settings);
            File.WriteAllText("settings.json", content);
        }
    }
}
