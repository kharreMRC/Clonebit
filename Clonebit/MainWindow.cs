using System;
using System.Collections.Generic;
using System.IO;
using System.Net.NetworkInformation;
using System.Threading;
using Clonebit;
using Gtk;
using MySqlConnector;
using Newtonsoft.Json;

public partial class MainWindow : Window
{
    // Ur so bad its insane
    public static Notebook mainNotebook;
    public static MenuBar menuBar;
    public static MenuItem logoutItem;
    public static MenuItem refreshItem;

    public static MySqlConnection connector;
    public static Thread pingThread;

    public static Dictionary<string, string> usbStatus;

    public static string[] addresses;

    public static string privateKeyPath;
    public static string NFSPath;

    public static bool[] activeStation;
    public static bool[] selectedStation;

    public MainWindow() : base(WindowType.Toplevel)
    {
        Build();

        usbStatus = new Dictionary<string, string>();

        privateKeyPath = "/home/kharre/.ssh/id_ed25519";
        NFSPath = "/media/shared_folder/";
        activeStation = new bool[6];
        selectedStation = new bool[6];
        addresses = new string[]
        {
            "192.168.1.11",
            "192.168.1.12",
            "192.168.1.13",
            "192.168.1.14",
            "192.168.1.15",
            "192.168.1.16"
        };

        for (int i = 0; i < addresses.Length; i++)
        {
            usbStatus.Add(addresses[i], "REMOVED");
        }

        VBox vBox = new VBox(false, 0);
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

        refreshItem = new MenuItem("Actualiser")
        {
            Sensitive = false
        };

        Menu accountMenu = new Menu();
        accountMenu.Append(logoutItem);
        accountMenu.Append(new SeparatorMenuItem());
        accountMenu.Append(quitItem);
        accountItem.Submenu = accountMenu;
        
        menuBar.Append(accountItem);
        menuBar.Append(settingsItem);
        menuBar.Append(refreshItem);
        this.Add(menuBar);
        menuBar.ShowAll();

        mainNotebook = new Notebook();
        vBox.PackEnd(mainNotebook, true, true, 0);
        mainNotebook.TabPos = PositionType.Left;
        mainNotebook.AppendPage(new LoginPage(), new Label("Connexion"));
        this.Add(mainNotebook);
        mainNotebook.ShowAll();
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
        for (int i = 0; i < addresses.Length; i++)
        {
            usbStatus[addresses[i]] = "REMOVED";
        }
        HomePage.stationStore.Clear();

        byte pageNo = (byte)mainNotebook.NPages;
        for (var i = 0; i < pageNo; i++)
        {
            // Delete current page at position 1
            mainNotebook.RemovePage(0);
        }

        // Show login page
        mainNotebook.AppendPage(new LoginPage(), new Label("Connexion"));
        mainNotebook.ShowAll();

        logoutItem.Sensitive = false;
        refreshItem.Sensitive = false;

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

    public static void GetStationStatus()
    {
        //addresses.AsParallel().ForAll(address => Console.WriteLine(address.Key + ":" + PingStation(address.Key)));
        GetUSBStatus();
        for (var i = 0; i < addresses.Length; i++)
        {
            Console.WriteLine($"[PING] {addresses[i]} : {PingStation(addresses[i])}");
            if (PingStation(addresses[i]).Equals("Success"))
            {
                HomePage.stationStore.AppendValues((i + 1).ToString(), addresses[i], usbStatus[addresses[i]]);
                mainNotebook.AppendPage(
                    new StationPage(addresses[i], (byte)(i + 1)),
                    new Label($"Station {i + 1}"));
                mainNotebook.ShowAll();
            }
        }
        Console.Write("\n");
    }

    public static void GetUSBStatus()
    {
        string content = File.ReadAllText("/media/shared_folder/LOG/usblog_14.json");

        var logData = JsonConvert.DeserializeObject<Dictionary<string, string>[]>(content);

        for (int i = 0; i < logData.Length; i++)
        {
            usbStatus[logData[i]["IP"]] = logData[i]["Action"];
        }

        foreach (var usb in usbStatus)
        {
            Console.WriteLine($"[USB] {usb.Key} : {usb.Value}");
        }
        Console.Write("\n");
    }

    public static string PingStation(string address)
    {
        Ping pingRequest = new Ping();
        PingReply pingReply = pingRequest.Send(address, 2);
        return pingReply.Status.ToString();
    }
}
