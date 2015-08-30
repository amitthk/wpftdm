using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace wpftdm
{
   internal class AppSettings
    {

        private static volatile AppSettings _instance;
        private static object syncRoot = new Object();
        private AppSettingsInfo _settingsInfo;

        wpftdm.Data.XmlPersister<AppSettingsInfo> _persister;

       private AppSettings()
       {
           _settingsInfo = new AppSettingsInfo();
           _persister = new Data.XmlPersister<AppSettingsInfo>("AppSettingsInfo.xml");
           _persister.load();

           getSettingsDefault();
       }

       private void getSettingsDefault()
       {
           if ((_settingsInfo.PomodoroDurationMinutes == 0)&&((_persister.objList==null)||(_persister.objList.Count<=0)))
           {

                   var tmp = new AppSettingsInfo() { PomodoroDurationMinutes=AppConstants.PomodoroDurationMinutesDefault, RestDurationMinutes=AppConstants.RestDurationMinutesDefault, CompletionSoundPath=AppConstants.CompletionSoundDefaultPath, TickerSoundPath=AppConstants.TickerSoundDefaultPath };
                   _persister.objList.Add(tmp);
                   _persister.save();

                   _settingsInfo.PomodoroDurationMinutes = AppConstants.PomodoroDurationMinutesDefault;
                   _settingsInfo.RestDurationMinutes = AppConstants.RestDurationMinutesDefault;
                   _settingsInfo.CompletionSoundPath = AppConstants.CompletionSoundDefaultPath;
                   _settingsInfo.TickerSoundPath = AppConstants.TickerSoundDefaultPath;
           }
           else
           {
               _settingsInfo = _persister.objList.FirstOrDefault();
           }
       }

       [XmlIgnore]
       public static AppSettings Instance
       {
           get
           {
               if (_instance == null)
               {
                   lock (syncRoot)
                   {
                       if (_instance == null)
                       {
                           _instance = new AppSettings();
                       }
                   }
               }

               return _instance;
           }
       }

       private void _Save()
       {
           //Use automapper
           _persister.objList.Clear();
           _persister.objList.Add(_settingsInfo);
           _persister.save();
       }

       public static void Save()
       {
           _instance._Save();
       }


       public int PomodoroDurationMinutes
       {
           get { return _settingsInfo.PomodoroDurationMinutes; }
           set { _settingsInfo.PomodoroDurationMinutes = value; }
       }

       public int RestDurationMinutes
       {
           get { return _settingsInfo.RestDurationMinutes; }
           set { _settingsInfo.RestDurationMinutes = value; }
       }


       private string _appDataPath=AppConstants.AppDataPath;

       public string AppDataPath
       {
           get { return _appDataPath; }
           set { _appDataPath = value; }
       }

    }
}
