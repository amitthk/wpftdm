using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wpftdm.ViewModels;

namespace wpftdm.Core
{
    class ViewModelLocator
    {
        public MainWindowViewModel MainWindowViewModel { get { return new MainWindowViewModel(); } }
        public HomeViewModel HomeViewModel { get { return new HomeViewModel(); } }
        public AppSettingsViewModel AppSettingsViewModel { get { return new AppSettingsViewModel(); } }
    }
}
