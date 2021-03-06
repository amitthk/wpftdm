﻿using log4net.Config;
using log4net.Repository.Hierarchy;
using Raven.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using wpftdm.Core;

namespace wpftdm
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static Logger _log;
        public static IDocumentSession DocumentSession;
        public static IEventAggregator EventAggregator { get; private set; }


        protected override void OnStartup(StartupEventArgs e)
        {
            Configure();
            ConfigureDatabase();
            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            using (DocumentSession)
            {
                if (DocumentSession!=null)
                {
                    DocumentSession.SaveChanges();
                }
            }
            base.OnExit(e);
        }

        private void ConfigureDatabase()
        {
            wpftdm.Data.TodoDatabase.Instance.Initialize();
            DocumentSession = wpftdm.Data.TodoDatabase.Instance.OpenSession();
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
            EventAggregator = new EventAggregator();
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
