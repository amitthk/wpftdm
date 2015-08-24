using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace wpftdm.Util
{
    public class NotifyIconHelper
    {
        private static NotifyIconHelper _instance;
        private NotifyIcon _notifyIcon;
        private bool MinimizeTrayHandlerAdded = false;
        private Window _window;

        //We want to have a single place for creation of NotifyIcon
        public static NotifyIconHelper Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new NotifyIconHelper();
                }
                return (_instance);
            }
        }

        private NotifyIconHelper()
        {
            // TODO: Complete member initialization
            buildNotifyIcon();
        }

        //Must set the instance of window
        public NotifyIcon GetIcon(Window win)
        {
            _window = win;
            return _notifyIcon;
        }


        private void buildNotifyIcon()
        {
            _notifyIcon = new NotifyIcon();
            ////Create instance of below Icon according to your preference like below.
            //new System.Drawing.Icon(System.Windows.Application.GetResourceStream(new Uri("pack://application:,,,/Resources/Favicon.ico")).Stream);
            var icon = System.Drawing.SystemIcons.Application;
            _notifyIcon.Icon = icon;
            _notifyIcon.Text = "Right-Click for Menu. Double-Click to Restore Application.";

            //Attach goodies to NotifyIcon

            if (!MinimizeTrayHandlerAdded)
            {
                _notifyIcon.DoubleClick +=
                    delegate(object sender, EventArgs args)
                    {
                        _notifyIcon.Visible = false;
                        _window.Show();
                        _window.WindowState = WindowState.Normal;
                    };
                _notifyIcon.ContextMenu = new System.Windows.Forms.ContextMenu();
                _notifyIcon.ContextMenu.MenuItems.Add(new System.Windows.Forms.MenuItem("Show Application", (sender, args) =>
                {
                    _notifyIcon.Visible = false;
                    _window.Show();
                    _window.WindowState = WindowState.Normal;
                }) { DefaultItem = true });
                _notifyIcon.ContextMenu.MenuItems.Add("-");
                _notifyIcon.ContextMenu.MenuItems.Add("Exit", (sender, args) => { _notifyIcon.Visible = false; _window.Close(); });
                _notifyIcon.BalloonTipText = "Right-Click for Menu. Double-Click to Restore Application.";

                MinimizeTrayHandlerAdded = true;
            }
        }
    }
}
