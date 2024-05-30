using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using TimeSyncApp.ViewModel;


namespace TimeSyncApp
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var configurationService = new ConfigurationService();
            var configuration = configurationService.LoadConfiguration();

            if (configuration == null || !configuration.IsConfigured)
            {                
                configuration = new ConfigurationModel { IsConfigured = true };
                configurationService.SaveConfiguration(configuration);
            }
        }
    }
}
