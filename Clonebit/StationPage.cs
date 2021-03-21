using Gtk;
using Pango;

namespace Clonebit
{
    [System.ComponentModel.ToolboxItem(true)]
    public partial class StationPage : Bin
    {
        private byte stationNo;

        public StationPage(string address, byte stationNum)
        {
            Build();
            titleLabel.Text = SetBold("STATION " + stationNum);
            titleLabel.UseMarkup = true;
            titleLabel.ModifyFont(FontDescription.FromString("36"));

            this.stationNo = stationNum;

            addressInfoLabel.Text = SetItalic(address);

            addressInfoLabel.UseMarkup = true;
        }

        protected void OnDuplicateCheckBoxClicked(object sender, System.EventArgs e)
        {
            if (duplicateCheckBox.Active)
            {
                titleLabel.ModifyFg(StateType.Normal, new Gdk.Color(0, 255, 0));
            }
            else
            {
                titleLabel.ModifyFg(StateType.Normal, new Gdk.Color(48, 48, 48));
            }
            MainWindow.activeStation[stationNo - 1] = !MainWindow.activeStation[stationNo - 1];

            MainWindow.selectedStation[stationNo - 1] = !MainWindow.selectedStation[stationNo - 1];
        }

        public string SetBold(string text)
        {
            return "<b>" + text + "</b>";
        }

        public string SetItalic(string text)
        {
            return "<i>" + text + "</i>";
        }
    }
}
