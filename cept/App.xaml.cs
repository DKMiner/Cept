using System.Windows.Forms;

namespace Cept
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        private NotifyIcon icon;
        private MenuItem autoAcceptMenuItem;
        private MenuItem keepOnMenuItem;
        private AutoAcceptManager autoAcceptManager;
        private LeagueConnection leagueConnection;

        public App()
        {
            autoAcceptMenuItem = new MenuItem("Auto Accept")
            {
                Checked = false
            };

            keepOnMenuItem = new MenuItem("Keep on after joining")
            {
                Checked = false
            };

            icon = new NotifyIcon
            {
                Text = "Cept! Auto Accept",
                Icon = Cept.Properties.Resources.line,
                Visible = true,
                ContextMenu = new ContextMenu(new []
                {
                    new MenuItem(Program.APP_NAME + " " + Program.VERSION)
                    {
                        Enabled = false
                    },
                    autoAcceptMenuItem,
                    keepOnMenuItem,
                    new MenuItem("Quit", (a, b) => Shutdown())
                })
            };

            leagueConnection = new LeagueConnection();

            leagueConnection.OnConnected += () =>
            {
                Dispatcher.Invoke(() =>
                {
                    icon.Icon = Cept.Properties.Resources.cept;
                });
            };

            leagueConnection.OnDisconnected += () =>
            {
                Dispatcher.Invoke(() =>
                {
                    icon.Icon = Cept.Properties.Resources.line;
                });
            };


            autoAcceptManager = new AutoAcceptManager();
            autoAcceptManager.AutoAcceptChanged += UpdateAutoAcceptMenuItem;
            UpdateAutoAcceptMenuItem(autoAcceptManager.AutoAcceptEnabled);

            autoAcceptMenuItem.Click += (sender, args) =>
            {
                autoAcceptManager.SetAutoAccept(!autoAcceptManager.AutoAcceptEnabled);
            };

            keepOnMenuItem.Click += (sender, args) =>
            {
                autoAcceptManager.SetKeepOn(!autoAcceptManager.KeepOn);
                keepOnMenuItem.Checked = autoAcceptManager.KeepOn;
            };
        }

        /**
         * Updates the menu item based on the current auto-accept state.
         */
        private void UpdateAutoAcceptMenuItem(bool enabled)
        {
            autoAcceptMenuItem.Checked = enabled;
        }

        /**
         * Shows a simple notification with the specified text for 5 seconds.
         */
        public void ShowNotification(string text)
        {
            icon.BalloonTipTitle = "Cept! Auto Accept";
            icon.BalloonTipText = text;
            icon.ShowBalloonTip(5000);
        }

    }
}
