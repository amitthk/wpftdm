using log4net.Config;
using log4net.Repository.Hierarchy;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Wintellect.Sterling.Core;

namespace wpftdm
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static Logger _log;
        public static SterlingEngine engine;
        public static ISterlingDatabaseInstance databaseInstance;
        private SterlingDefaultLogger _logger;
        private ISterlingDriver _driver;
        private ISterlingPlatformAdapter _adapter;

        protected override void OnStartup(StartupEventArgs e)
        {
            Configure();
            ConfigureDatabase();
            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            if (_logger != null)
            {
                _logger.Detach();
            }
            
            engine.Dispose();
            databaseInstance = null;
            base.OnExit(e);
        }

        private void ConfigureDatabase()
        {
            _adapter = new Wintellect.Sterling.Server.PlatformAdapter();
            engine = new SterlingEngine(_adapter);
            engine.Activate();
            _driver= new Wintellect.Sterling.Server.FileSystem.FileSystemDriver(AppConstants.AppDataPath);
            databaseInstance = engine.SterlingDatabase.RegisterDatabase<Data.TodoDatabaseInstance>("WpfdtmDb", _driver);
        }

         void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            // Process unhandled exception
            _log.Log(log4net.Core.Level.Error,"Error Encountered: ",e.Exception);
            // Prevent default unhandled exception processing
            //e.Handled = true;
        }

        private void Configure()
        {
            _log = ((log4net.Repository.Hierarchy.Hierarchy)log4net.LogManager.GetRepository()).Root;
            _log.AddAppender(CreateFileAppender("FileAppender", System.IO.Path.Combine(Environment.CurrentDirectory, "wpftdm.log")));
            log4net.GlobalContext.Properties["LogFolderLocation"] = Environment.CurrentDirectory;
            BasicConfigurator.Configure();
        }

        // Create a new file appender
        public static log4net.Appender.IAppender CreateFileAppender(string name,
        string fileName)
        {
            log4net.Appender.FileAppender appender = new
          log4net.Appender.FileAppender();
            appender.Name = name;
            appender.File = fileName;
            appender.AppendToFile = true;

            log4net.Layout.PatternLayout layout = new
            log4net.Layout.PatternLayout();
            layout.ConversionPattern = "%d [%t] %-5p %c [%x] - %m%n";
            layout.ActivateOptions();

            appender.Layout = layout;
            appender.ActivateOptions();

            return appender;
        }
    }
}
