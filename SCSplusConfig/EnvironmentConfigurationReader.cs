using SC.SimplSharp.Config;
using SCConfigSplus.JSON;
using SCConfigSplus.Readers;
using SSMono.IO;

namespace SCSplusConfig
{
    

    public class EnvironmentConfigurationReader
    {
        private ConfigurationReader<EnvironmentControls> _configurationReader;
        private JsonSettingsReader _settingsReader;
        private FileSystemWatcher _watcher;

        /// <summary>
        /// Delegate for S+ for OnConfigurationChanged event.
        /// </summary>
        /// <param name="sender">Object that triggered the event</param>
        /// <param name="config">Config to send to S+</param>
        public delegate void EnvControlsConfigChangedDel(object sender, EnvironmentControls config);

        /// <summary>
        /// Event to update configuration when the file changes.
        /// </summary>
        public event EnvControlsConfigChangedDel OnConfigurationChanged;


        /// <summary>
        /// Method to initialize the system.
        /// </summary>
        /// <param name="path">Path to file to be read</param>
        public void Initialize(string path)
        {
            _settingsReader = new JsonSettingsReader(path);

            _configurationReader = new ConfigurationReader<EnvironmentControls>(_settingsReader);

            _watcher = new FileSystemWatcher();

            ConfigureFileSystemWatcher(path);
        }

        /// <summary>
        /// Method to read the EnvironmentControls settings
        /// </summary>
        /// <returns>EnvironmentControls indicating the stored settings</returns>
        public void ReadSettings()
        {
            var settings = _configurationReader.ReadSettings();

            FireOnConfigChangedEvent(settings);
        }

        private void ConfigureFileSystemWatcher(string path)
        {
            var directory = Path.GetDirectoryName(path);
            var fileName = Path.GetFileName(path);

            _watcher = new FileSystemWatcher
            {
                Path = directory,
                NotifyFilter = NotifyFilters.LastWrite,
                Filter = fileName
            };

            _watcher.Changed += OnWatcherChanged;

            _watcher.EnableRaisingEvents = true;
        }

        /// <summary>
        /// Updates the stored configuration when the FileSystemWatcher determines that the file has changed.
        /// </summary>
        /// <param name="sender">Watcher that fired the event</param>
        /// <param name="e">Arguments</param>
        private void OnWatcherChanged(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType != WatcherChangeTypes.Changed)
            {
                return;
            }

            var settings = _configurationReader.ReadSettings();

            FireOnConfigChangedEvent(settings);
        }


        /// <summary>
        /// Fires the event for S+ to send the data to the rest of the program.
        /// </summary>
        /// <param name="settings">Object to send to S+</param>
        private void FireOnConfigChangedEvent(EnvironmentControls settings)
        {
            var onFileChangedHandler = OnConfigurationChanged;

            if (onFileChangedHandler != null)
            {
                onFileChangedHandler(this, settings);
            }
        }
    }
}
