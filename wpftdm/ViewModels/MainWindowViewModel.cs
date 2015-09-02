using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using wpftdm.Util;

namespace wpftdm.ViewModels
{
    public class MainWindowViewModel
    {
        private readonly ICommand _ShowHelpCmd;

        public ICommand ShowHelpCmd { get { return (_ShowHelpCmd); } }

        private readonly ICommand _ExitAppCmd;

        public ICommand ExitAppCmd { get { return (_ExitAppCmd); } }

        private readonly ICommand _MinimizeToTrayCmd;

        public ICommand MinimizeToTrayCmd { get { return (_MinimizeToTrayCmd); } }

        private readonly ICommand _ShowAppSettingsCmd;

        public ICommand ShowAppSettingsCmd { get { return (_ShowAppSettingsCmd); } }

        private readonly ICommand _GoToHomeCmd;

        public ICommand GoToHomeCmd { get { return (_GoToHomeCmd); } }


        private void ExecGoToHome(object obj)
        {
            //Todo: Add the functionality for GoToHomeCmd Here
            App.EventAggregator.Publish<wpftdm.Core.NavMessage>(new wpftdm.Core.NavMessage("Home"));
        }

        [DebuggerStepThrough]
        private bool CanGoToHome(object obj)
        {
            //Todo: Add the checking for CanGoToHome Here
            return (true);
        }

        public MainWindowViewModel()
        {
            _ExitAppCmd = new RelayCommand(ExecExitApp, CanExitApp);
            _MinimizeToTrayCmd = new RelayCommand(ExecMinimizeToTrayCmd, CanMinimizeToTray);
            _ShowAppSettingsCmd = new RelayCommand(ExecShowAppSettings, CanShowAppSettings);
            _ShowHelpCmd = new RelayCommand(ExecShowHelp, CanShowHelp);
            _GoToHomeCmd = new RelayCommand(ExecGoToHome, CanGoToHome);

        }

        private void ExecShowHelp(object obj)
        {
            //Todo: Add the functionality for ShowHelpCmd Here
            var helpPop = new wpftdm.Views.Help();
            helpPop.ShowDialog();
        }

        [DebuggerStepThrough]
        private bool CanShowHelp(object obj)
        {
            //Todo: Add the checking for CanShowHelp Here
            return (true);
        }

        private void ExecShowAppSettings(object obj)
        {
            //Todo: Add the functionality for ShowAppSettingsCmd Here
            var catOpts = new AppSettingsWindow();
            catOpts.ShowDialog();
        }

        [DebuggerStepThrough]
        private bool CanShowAppSettings(object obj)
        {
            //Todo: Add the checking for CanShowAppSettings Here
            return (true);
        }


        private void ExecExitApp(object obj)
        {
            System.Windows.Application.Current.Shutdown();
        }

        //These methods are checked again & again in loop, so we will step through them while debugging
        [DebuggerStepThrough]
        private bool CanMinimizeToTray(object obj)
        {
            return (true);
        }

        private void ExecMinimizeToTrayCmd(object obj)
        {
            var win = (Window)obj;

            var notifyIcon = NotifyIconHelper.Instance.GetIcon(win);

            notifyIcon.Visible = true;
            win.Hide();
        }

        [DebuggerStepThrough]
        private bool CanExitApp(object obj)
        {
            //Todo: Add the checking for CanExitApp Here
            return (true);
        }

    }
}
