using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace CustomProtocolApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            RegisterProtocol();

            MainWindow mainWindow = new MainWindow();
            if (e.Args.Length > 0)
            {
                string arguments = string.Join(", ", e.Args);
                mainWindow.ArgumentsText = "Arguments: " + arguments;
            }
            else
            {
                mainWindow.ArgumentsText = "No arguments provided.";
            }

            mainWindow.Show();
        }

        private void RegisterProtocol()
        {
            string customProtocol = "CustomProtocol";

            // Get the installation path of the application
            string applicationPath = Process.GetCurrentProcess().MainModule.FileName;

            // Check if the custom protocol already exists
            RegistryKey key = Registry.ClassesRoot.OpenSubKey(customProtocol);

            if (key == null) // If the protocol is not registered we need to register it  
            {
                key = Registry.ClassesRoot.CreateSubKey(customProtocol);
                key.SetValue(string.Empty, "URL: " + customProtocol);
                key.SetValue("URL Protocol", string.Empty);

                key = key.CreateSubKey(@"shell\open\command");
                key.SetValue(string.Empty, applicationPath + " " + "%1");   //%1 is the parameter that windows can pass to the application 

            }

            key.Close();
        }
    }
}
