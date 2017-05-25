using System.Collections.Generic;
using SC.SimplSharp.Config;
using SCConfigSplus.JSON;
using SCConfigSplus.Readers;
using SSMono.IO;

namespace SCSplusConfig
{
    public class SourceConfigurationReader
    {
        private const string SectionName = "Sources";

        private ConfigurationReader<List<Source>> _configurationReader;
        private JsonSettingsReader _settingsReader;
        private FileSystemWatcher _watcher;
        private List<Source> _sources = new List<Source>();

        public delegate void SourcesConfigChangedDel(ushort index, Source source);

        public SourcesConfigChangedDel OnConfigurationChanged { get; set; }

        public void Initialize(string path)
        {
            _settingsReader = new JsonSettingsReader(path);

            _configurationReader = new ConfigurationReader<List<Source>>(_settingsReader);

            _watcher = new FileSystemWatcher();

            ConfigureFileSystemWatcher(path);
        }

        public void ReadSettings()
        {
            _sources = _configurationReader.ReadSettings(SectionName);

            FireOnConfigChangedEvent(_sources);
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

        private void OnWatcherChanged(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType != WatcherChangeTypes.Changed)
            {
                return;
            }

            var settings = _configurationReader.ReadSettings();

            FireOnConfigChangedEvent(settings);
        }

        private void FireOnConfigChangedEvent(List<Source> settings)
        {
            var handler = OnConfigurationChanged;

            if (OnConfigurationChanged == null)
            {
                return;
            }

            for (ushort i = 0; i < settings.Count; i++)
            {
                handler(i, settings[i]);
            }
        }
    }
}