using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Core2.Common
{
    public class AppSettings
    {
        private IConfigurationRoot config;
        private AppSettings()
        {
            ConfigurationBuilder builder = new ConfigurationBuilder();
            config = builder.SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true).Build();
            FileSystemWatcher watcher = new FileSystemWatcher(Directory.GetCurrentDirectory(), "appsettings.json");
            watcher.Changed += Watcher_Changed;
        }

        private void Watcher_Changed(object sender, FileSystemEventArgs e)
        {
            try
            {
                ConfigurationBuilder builder = new ConfigurationBuilder();
                config = builder.SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true).Build();
            }
            catch(Exception et)
            {

            }
        }

        private static AppSettings settins;

        static AppSettings()
        {
            settins = new AppSettings();
        }

        public static string Config(string key)
        {
            return settins[key];
        }
        private string this[string key]
        {
            get
            {
                string value = config[key];

                return string.IsNullOrEmpty(value) ? null : value;
            }
        }
    }
}
