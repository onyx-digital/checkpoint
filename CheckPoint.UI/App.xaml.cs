using NLog;
using CheckPoint.UI.Controllers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace CheckPoint.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        static public Logger AppLogger;

        static public GitController GitControl;
        static public ProjectController ProjectControl;

        public App()
        {
            ConfigureLogging();
            AppLogger.Info("App Starting.");

            GitControl = new GitController();
            ProjectControl = new ProjectController();
        }

        private void ConfigureLogging()
        {
            AppLogger = NLog.LogManager.GetCurrentClassLogger();
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            AppLogger.Info("App Exiting.");
        }
    }
}
