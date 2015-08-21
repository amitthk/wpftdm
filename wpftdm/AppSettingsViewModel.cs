using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace wpftdm
{
    public class AppSettingsViewModel:BaseViewModel
    {
        public AppSettingsViewModel()
        {
            _PomodoroDurationMinutes = AppSettings.Instance.PomodoroDurationMinutes;
            _RestDurationMinutes = AppSettings.Instance.RestDurationMinutes;
            _SaveAppSettingsCmd = new RelayCommand(ExecSaveAppSettingsCmd, CanSaveAppSettingsCmd);
        }

        private int _PomodoroDurationMinutes;

        public int PomodoroDurationMinutes
        {
            get { return _PomodoroDurationMinutes; }
            set
            {
                if (_PomodoroDurationMinutes != value)
                {
                    _PomodoroDurationMinutes = value;
                    OnPropertyChanged("PomodoroDurationMinutes");
                }
            }
        }

        private int _RestDurationMinutes;

        public int RestDurationMinutes
        {
            get { return _RestDurationMinutes; }
            set
            {
                if (_RestDurationMinutes != value)
                {
                    _RestDurationMinutes = value;
                    OnPropertyChanged("RestDurationMinutes");
                }
            }
        }

        private readonly ICommand _SaveAppSettingsCmd;

        public ICommand SaveAppSettingsCmd { get { return (_SaveAppSettingsCmd); } }


        private void ExecSaveAppSettingsCmd(object obj)
        {
            //Todo: Add the functionality for SaveAppSettingsCmdCmd Here
            AppSettings.Instance.PomodoroDurationMinutes = PomodoroDurationMinutes;
            AppSettings.Instance.RestDurationMinutes = RestDurationMinutes;
            AppSettings.Save();
            System.Windows.MessageBox.Show("Settings were saved!", "Settings Saved!", System.Windows.MessageBoxButton.OK);
        }

        [DebuggerStepThrough]
        private bool CanSaveAppSettingsCmd(object obj)
        {
            //Todo: Add the checking for CanSaveAppSettingsCmd Here
            return (true);
        }

        public string AppBasePath { get { return(AppConstants.AppBasePath); } }
        public string DataPath { get { return (AppConstants.AppDataPath); } }
    }
}
