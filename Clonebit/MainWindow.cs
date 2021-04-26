using System;
using System.Collections.Generic;
using System.IO;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Clonebit;
using Gtk;
using MySqlConnector;
using Newtonsoft.Json;

public partial class MainWindow : Window
{
    // Ur so bad its insane
    public static VBox vBox;
    public static MenuBar menuBar;
    public static MenuItem logoutItem;

    public static Clonebit.Settings settings;

    public static MySqlConnection connector;
    public static Thread pingThread;

    public static Dictionary<string, string> stationLog;
    public static Dictionary<string, string> usbLog;
    public static Dictionary<string, string> usbStatus;

    public MainWindow() : base(WindowType.Toplevel)
    {
        Build();

        string content = File.ReadAllText("settings.json");
        settings = JsonConvert.DeserializeObject<Clonebit.Settings>(content);

        stationLog = new Dictionary<string, string>();
        usbStatus = new Dictionary<string, string>();

        Console.WriteLine(settings.Addresses.Count);
        foreach (var address in settings.Addresses)
        {
            Console.WriteLine(address);
        }

        for (var i = 0; i < settings.Addresses.Count; i++)
        {
            stationLog.Add(settings.Addresses[i], "");
            usbStatus.Add(settings.Addresses[i], "NONE");
        }

        vBox = new VBox(false, 0);
        Add(vBox);
        vBox.ShowAll();

        menuBar = new MenuBar();
        vBox.PackStart(menuBar, false, false, 0);

        MenuItem accountItem = new MenuItem("Compte");
        logoutItem = new MenuItem("Déconnexion")
        {
            Sensitive = false
        };
        logoutItem.Activated += OnLogoutActivated;

        MenuItem quitItem = new MenuItem("Quitter");
        quitItem.Activated += OnQuitActivated;

        MenuItem settingsItem = new MenuItem("Paramètres");
        settingsItem.Activated += OnSettingsActivated;

        Menu accountMenu = new Menu();
        accountMenu.Append(logoutItem);
        accountMenu.Append(new SeparatorMenuItem());
        accountMenu.Append(quitItem);
        accountItem.Submenu = accountMenu;

        menuBar.Append(accountItem);
        menuBar.Append(settingsItem);
        Add(menuBar);
        menuBar.ShowAll();

        vBox.PackEnd(new LoginPage(), true, true, 0);
        vBox.ShowAll();
    }

    protected void OnDeleteEvent(object sender, DeleteEventArgs a)
    {
        // Abort else it crashes because the thread could be working
        pingThread.Abort();
        connector.Close();
        Application.Quit();
        a.RetVal = true;
    }

    protected void OnQuitActivated(object sender, EventArgs e)
    {
        // Abort else it crashes because the thread could be working
        pingThread.Abort();
        connector.Close();
        Application.Quit();
    }

    protected void OnLogoutActivated(object sender, EventArgs e)
    {
        for (int i = 0; i < settings.Addresses.Count; i++)
        {
            usbStatus[settings.Addresses[i]] = "NONE";
        }
        HomePage.stationStore.Clear();

        // Destroy home page and add login page
        vBox.Children[1].Destroy();

        vBox.PackEnd(new LoginPage(), true, true, 0);
        vBox.ShowAll();

        logoutItem.Sensitive = false;

        SettingsWindow.logStatus = false;

        pingThread.Abort();

        // Logout from the database
        connector.Close();
    }

    protected void OnSettingsActivated(object sender, EventArgs e)
    {
        SettingsWindow settingsWindow = new SettingsWindow();
        settingsWindow.Show(); 
    }

    protected void OnRefreshActivated(object sender, EventArgs e)
    {
    }

    public static async Task AsyncExecuteSQLCommand(string cmd)
    {
        var sqlCommand = new MySqlCommand(cmd, connector);
        await sqlCommand.ExecuteNonQueryAsync();
    }

    public static void ExecuteSQLCommand(string cmd)
    {
        var sqlCommand = new MySqlCommand(cmd, connector);
        sqlCommand.ExecuteNonQuery();
    }

    public static async void GetStationStatus()
    {
        for (var i = 0; i < settings.Addresses.Count; i++)
        {
            var stationNo = i + 11;
            var logPath = new StringBuilder($"{settings.LogsPath}usblog_{stationNo.ToString()}.json").ToString();

            if (PingStation(settings.Addresses[i]).Equals("Success"))
            {
                Console.WriteLine($"[PING] {settings.Addresses[i]} : Success");
                GetUSBStatus(logPath, settings.Addresses[i]);
                await AsyncExecuteSQLCommand($"UPDATE duplication_station SET ds_lastusbserial='{GetUSBSerial(logPath, settings.Addresses[i])}' WHERE ds_ip='{settings.Addresses[i]}';");
                HomePage.stationStore.AppendValues((i + 1).ToString(), settings.Addresses[i], usbStatus[settings.Addresses[i]]);
            }
            else
            {
                Console.WriteLine($"[PING] {settings.Addresses[i]} : Fail");
            }
        }
        Console.Write("\n");
    }

    public static void GetUSBStatus(string logPath, string address)
    {
        try
        {
            string content = File.ReadAllText(logPath);
            var logData = JsonConvert.DeserializeObject<Dictionary<string, string>[]>(content);

            if (logData[logData.Length - 1]["Action"].Equals("ADDED"))
            {
                usbStatus[address] = "Oui";
                Console.WriteLine($"[USB] {address} : OK");
            }
            else
            {
                usbStatus[address] = "Non";
                Console.WriteLine($"[USB] {address} : NO USB");
            }
        }
        catch (Exception)
        {
            usbStatus[address] = "Aucun journal";
            Console.WriteLine($"[USB] {address} : NO LOG");
        }
    }

    public static string GetUSBSerial(string logPath, string address)
    {
        try
        {
            string content = File.ReadAllText(logPath);
            var logData = JsonConvert.DeserializeObject<Dictionary<string, string>[]>(content);
            return logData[logData.Length - 1]["SerialNumber"];
        }
        catch (Exception)
        {
            var serial = "";
            using (var cmd = new MySqlCommand($"SELECT ds_lastusbserial FROM duplication_station WHERE ds_ip='{address}';", connector))
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    serial = reader.GetString(0);
                }
            }
            return "";
        }
    }

    public static string PingStation(string address)
    {
        Ping pingRequest = new Ping();
        PingReply pingReply = pingRequest.Send(address, 2);
        return pingReply.Status.ToString();
    }

    // Unused
    //public static async Task AsyncExecuteSQLCommand(string cmd)
    //{
    //    var sqlCommand = new MySqlCommand(cmd, connector);
    //    await sqlCommand.ExecuteNonQueryAsync();
    //}
}
