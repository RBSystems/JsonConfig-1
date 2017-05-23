using System.Linq;

namespace SC.SimplSharp.Utilities
{
    public class SourceConfigurationUpdater
    {
        private int _maxNumberOfSources;
        public UpdateSourceInformationDelegate UpdateSourceInformation { get; set; }
        public UpdateSourceCountDelegate UpdateSourceCount { get; set; }
        public UpdateSourceEnabledCountDelegate UpdateSourceEnabledCount { get; set; }
        public UpdateSelectedSourceDelegate UpdateSelectedSource { get; set; }

        public delegate void UpdateSelectedSourceDelegate(Source source);
        public delegate void UpdateSourceInformationDelegate(ushort id, Source source);
        public delegate void UpdateSourceCountDelegate(ushort count);
        public delegate void UpdateSourceEnabledCountDelegate(ushort count);

        public SourceConfigurationUpdater()
        {
            _maxNumberOfSources = 10;
            ConfigurationLoader.OnConfigurationLoaded += ConfigurationLoader_OnConfigurationLoaded;
            ConfigurationLoader.OnConfigurationSaved += ConfigurationLoader_OnConfigurationSaved;
        }

        private void ConfigurationLoader_OnConfigurationSaved()
        {
            UpdateSPlusSourceCount((ushort) ConfigurationLoader.Config.Sources.Count);
            UpdateSPlusEnabledSourceCount((ushort)ConfigurationLoader.Config.Sources.Count(s => s.Enabled == 1)); 
            UpdateSPlusSourceInformation();
        }

        private void ConfigurationLoader_OnConfigurationLoaded()
        {
            UpdateSPlusSourceCount((ushort) ConfigurationLoader.Config.Sources.Count);
            UpdateSPlusEnabledSourceCount((ushort)ConfigurationLoader.Config.Sources.Count(s => s.Enabled == 1));
            UpdateSPlusSourceInformation();
        }

        public void Initialize(int maxNumberOfSources)
        {
            _maxNumberOfSources = maxNumberOfSources;
        }

        public void AddSource(Source itemToAdd)
        {
            if (ConfigurationLoader.Config.Sources.Count <= _maxNumberOfSources)
            {
                ConfigurationLoader.Config.Sources.Add(itemToAdd);
                ConfigurationLoader.Config.NumberOfInputs = ConfigurationLoader.Config.Sources.Count;
            }

            ConfigurationLoader.ConfigChanged = true;

            UpdateSPlusSourceCount( (ushort)ConfigurationLoader.Config.Sources.Count);
            UpdateSPlusSourceInformation();
        }

        public void RemoveSource(ushort index)
        {
            ConfigurationLoader.Config.Sources.RemoveAt(index - 1);
            ConfigurationLoader.Config.NumberOfInputs = ConfigurationLoader.Config.Sources.Count;

            ConfigurationLoader.ConfigChanged = true;

            UpdateSPlusSourceCount((ushort) ConfigurationLoader.Config.Sources.Count);
            UpdateSPlusSourceInformation();

        }

        public void UpdateSource(ushort index, Source itemToUpdate)
        {
            ConfigurationLoader.Config.Sources[index - 1] = itemToUpdate;

            ConfigurationLoader.ConfigChanged = true;

            UpdateSPlusSourceInformation();
        }

        public void SaveSourceConfiguration()
        {
            ConfigurationLoader.SaveConfigurationFile(ConfigurationLoader.FileName);
        }

        public void GetSource(ushort index)
        {
            if (UpdateSelectedSource != null)
            {
                UpdateSelectedSource(ConfigurationLoader.Config.Sources[index -1]);
            }
        }

        private void UpdateSPlusSourceCount(ushort count)
        {
            if (UpdateSourceCount != null)
            {
                UpdateSourceCount(count);
            }
        }

        private void UpdateSPlusEnabledSourceCount(ushort count)
        {
            if (UpdateSourceEnabledCount != null)
            {
                UpdateSourceEnabledCount(count);
            }
        }

        private void UpdateSPlusSourceInformation()
        {
            for (var i = 0; i < ConfigurationLoader.Config.Sources.Count; i++)
            {
                if (UpdateSourceInformation != null)
                {
                    UpdateSourceInformation((ushort)(i + 1), ConfigurationLoader.Config.Sources[i]);
                }
            }
        }
    }
}