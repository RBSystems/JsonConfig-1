using SC.SimplSharp.Config;
using SCConfigSplus.JSON;
using SCSplusConfig.Writers;

namespace SCSplusConfig
{
    /// <summary>
    /// Class to write 
    /// </summary>
    public class EnvironmentConfigurationWriter
    {
        private ConfigurationWriter<EnvironmentControls> _configWriter;
        private JsonSettingsWriter _settingsWriter;

        /// <summary>
        /// Method to initialize the writer.
        /// </summary>
        /// <param name="path">File to save settings to.</param>
        public void Initialize(string path)
        {
            _settingsWriter = new JsonSettingsWriter(path);

            _configWriter = new ConfigurationWriter<EnvironmentControls>(_settingsWriter);
        }

        /// <summary>
        /// Method to save settings to specified file
        /// </summary>
        /// <param name="settings">Object to save</param>
        public void SaveSettings(EnvironmentControls settings)
        {
            _configWriter.SaveSettings(settings);
        }
    }
}