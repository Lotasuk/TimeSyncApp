using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace TimeSyncApp.ViewModel
{
    public class ConfigurationService
    {
        private const string ConfigFileName = "C:\\config.dat";

        public ConfigurationModel LoadConfiguration()
        {
            if (File.Exists(ConfigFileName))
            {
                using (var stream = File.OpenRead(ConfigFileName))
                {
                    var formatter = new BinaryFormatter();
                    return (ConfigurationModel)formatter.Deserialize(stream);
                }
            }
            return null;
        }

        public void SaveConfiguration(ConfigurationModel config)
        {
            using (var stream = File.Create(ConfigFileName))
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, config);
            }
        }
    }
}
