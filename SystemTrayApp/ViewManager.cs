using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;

namespace SystemTrayApp
{
    public class ViewManager
    {
        public ViewManager(IConnectionsManager connectionManger)
        {

            System.Diagnostics.Debug.Assert(connectionManger != null);
            _connectionsManager = connectionManger;

            _components = new System.ComponentModel.Container();
            _notifyIcon = new System.Windows.Forms.NotifyIcon(_components)
            {
                ContextMenuStrip = new ContextMenuStrip(),
                Icon = SystemTrayApp.Properties.Resources.ReadyIcon,
                Text = "Global Protect Connection Switcher",
                Visible = true,
            };

            _notifyIcon.ContextMenuStrip.Opening += ContextMenuStrip_Opening;
            _notifyIcon.DoubleClick += notifyIcon_DoubleClick;
            _notifyIcon.MouseUp += notifyIcon_MouseUp;
            _aboutViewModel = new WpfFormLibrary.ViewModel.AboutViewModel();
            _settingsViewModel = new WpfFormLibrary.ViewModel.SettingsViewModel(
                   _connectionsManager.GetExistingConnections,
                   _connectionsManager.RemoveConnection,
                   _connectionsManager.AddNewConnection
             );
            _aboutViewModel.Icon = _settingsViewModel.Icon;

            _hiddenWindow = new System.Windows.Window();
            _hiddenWindow.Hide();
        }

        System.Windows.Media.ImageSource AppIcon
        {
            get
            {
                System.Drawing.Icon icon = Properties.Resources.ReadyIcon;
                return System.Windows.Interop.Imaging.CreateBitmapSourceFromHIcon(
                    icon.Handle, 
                    System.Windows.Int32Rect.Empty, 
                    System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());
            }
        }

        // This allows code to be run on a GUI thread
        private System.Windows.Window _hiddenWindow;

        private System.ComponentModel.IContainer _components;
        // The Windows system tray class
        private NotifyIcon _notifyIcon;  
        private IConnectionsManager _connectionsManager;

        private WpfFormLibrary.View.AboutView _aboutView;
        private WpfFormLibrary.ViewModel.AboutViewModel _aboutViewModel;

        private WpfFormLibrary.View.SettingsView _settingsView;
        private WpfFormLibrary.ViewModel.SettingsViewModel _settingsViewModel;


        //private ToolStripMenuItem _startDeviceMenuItem;
        //private ToolStripMenuItem _stopDeviceMenuItem;
        //private ToolStripMenuItem _exitMenuItem;

        private void DisplayStatusMessage(string text)
        {
            _hiddenWindow.Dispatcher.Invoke(delegate
            {
                _notifyIcon.BalloonTipText = "null" + ": " + text;
                // The timeout is ignored on recent Windows
                _notifyIcon.ShowBalloonTip(3000);
            });
        }

        private void UpdateSettingsView() { }
        
        private ToolStripMenuItem ToolStripMenuItemWithHandler(string displayText, string tooltipText, EventHandler eventHandler)
        {
            var item = new ToolStripMenuItem(displayText);
            if (eventHandler != null)
            {
                item.Click += eventHandler;
            }

            item.ToolTipText = tooltipText;
            return item;
        }
        
        private void ShowSettingsView_Click(object sender, EventArgs e)
        {
            if (_settingsView == null)
            {
                _settingsView = new WpfFormLibrary.View.SettingsView();
                _settingsView.DataContext = _settingsViewModel;

                _settingsView.Closing += ((arg_1, arg_2) => _settingsView = null);
                _settingsView.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
                _settingsView.Show();
                UpdateSettingsView();
            }
            else
            {
                _settingsView.Activate();
            }
            _settingsView.Icon = AppIcon;
        }

        private void CopyConnectionFiles_Click(object sender, EventArgs e)
        {
           
        }


        private void ShowAboutView()
        {
            if (_aboutView == null)
            {
                _aboutView = new WpfFormLibrary.View.AboutView();
                _aboutView.DataContext = _aboutViewModel;
                _aboutView.Closing += ((arg_1, arg_2) => _aboutView = null);
                _aboutView.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;

                _aboutView.Show();
            }
            else
            {
                _aboutView.Activate();
            }
            _aboutView.Icon = AppIcon;

            _aboutViewModel.AddVersionInfo("Hardware", "null");
            _aboutViewModel.AddVersionInfo("Version", Assembly.GetExecutingAssembly().GetName().Version.ToString());
            _aboutViewModel.AddVersionInfo("Serial Number", "142573462354");
        }

        private void showHelpItem_Click(object sender, EventArgs e)
        {
            ShowAboutView();
        }

        private void notifyIcon_DoubleClick(object sender, EventArgs e)
        {
            ShowAboutView();
        }

        private void notifyIcon_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                MethodInfo mi = typeof(NotifyIcon).GetMethod("ShowContextMenu", BindingFlags.Instance | BindingFlags.NonPublic);
                mi.Invoke(_notifyIcon, null);
            }
        }





        private void ContextMenuStrip_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = false;
            var userContextUserName = Environment.UserName;

            // TODO: Implement some checks to ensure that this folder actually exists / create if not exist.
            var connectionsList = _connectionsManager.GetExistingConnections();

            if (_notifyIcon.ContextMenuStrip.Items.Count == 0)
            {
                if (connectionsList.Any())
                {
                    foreach (var item in connectionsList)
                    {
                        var itemLabel = item.Split('\\').Last();
                        ToolStripMenuItem menuItem = new ToolStripMenuItem(itemLabel);
                        menuItem.DropDownItems.Add(ToolStripMenuItemWithHandler("Enable", "Enables Connection in Global Protect", CopyConnectionFiles_Click));
                        _notifyIcon.ContextMenuStrip.Items.Add(menuItem);
                       
                    }
                }
                
               
                _notifyIcon.ContextMenuStrip.Items.Add(new ToolStripSeparator());
                _notifyIcon.ContextMenuStrip.Items.Add(ToolStripMenuItemWithHandler("Settings", "Configure and edit connections", ShowSettingsView_Click));
                _notifyIcon.ContextMenuStrip.Items.Add(ToolStripMenuItemWithHandler("&About", "Shows the About dialog", showHelpItem_Click));
            }
        }
    }
}
