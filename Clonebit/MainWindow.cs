using System;
using System.Threading;
using Clonebit;
using Gtk;
using MySqlConnector;

public partial class MainWindow : Window
{
    // Ur so bad its insane
    public static Notebook mainNotebook;
    public static MenuBar menuBar;
    public static MenuItem logoutItem;

    public static MySqlConnection connector;
    public static Thread pingThread;

    public static string[] addresses;

    public static string privateKeyPath;
    public static string NFSPath;

    public static bool[] activeStation;
    public static bool[] selectedStation;

    public MainWindow() : base(WindowType.Toplevel)
    {
        Build();

        VBox vBox = new VBox(false, 0);
        Add(vBox);
        vBox.ShowAll();

        menuBar = new MenuBar();
        vBox.PackStart(menuBar, false, false, 0);

        MenuItem accountItem = new MenuItem("Compte");
        logoutItem = new MenuItem("Déconnexion");
        logoutItem.Activated += OnLogoutActivated;
        logoutItem.Sensitive = false;
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
        this.Add(menuBar);
        menuBar.ShowAll();

        mainNotebook = new Notebook();
        vBox.PackEnd(mainNotebook, true, true, 0);
        mainNotebook.TabPos = PositionType.Left;
        mainNotebook.AppendPage(new LoginPage(), new Label("Connexion"));
        this.Add(mainNotebook);
        mainNotebook.ShowAll();

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
        byte pageNo = (byte)mainNotebook.NPages;
        for (var i = 0; i < pageNo; i++)
        {
            // Delete current page at position 1
            mainNotebook.RemovePage(0);
        }

        // Show login page
        mainNotebook.AppendPage(new LoginPage(), new Label("Connexion"));
        mainNotebook.ShowAll();

        logoutAction.Sensitive = false;

        pingThread.Abort();

        // Logout from the database
        connector.Close();
    }

    protected void OnSettingsActivated(object sender, EventArgs e)
    {
        SettingsWindow settingsWindow = new SettingsWindow(addresses);
        settingsWindow.Show(); 
    }
}
