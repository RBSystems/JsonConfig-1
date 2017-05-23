using Crestron.SimplSharp;
using Crestron.SimplSharp.Reflection;

namespace SC.SimplSharp.Utilities
{
    

    public class DisplayConfigurationUpdater
    {
        private int _maxNumberOfDisplays;

        public UpdateDisplayInformationDelegate UpdateDisplayInformation { get; set; }
        public UpdateDisplayCountDelegate UpdateDisplayCount { get; set; }
        public UpdateSelectedDisplayDelegate UpdateSelectedDisplay { get; set; }

        public delegate void UpdateSelectedDisplayDelegate(Display display);
        public delegate void UpdateDisplayInformationDelegate(ushort id, Display display);
        public delegate void UpdateDisplayCountDelegate(ushort count);

        public DisplayConfigurationUpdater()
        {
            _maxNumberOfDisplays = 10;
            ConfigurationLoader.OnConfigurationLoaded += ConfigurationLoader_OnConfigurationLoaded;
            ConfigurationLoader.OnConfigurationSaved += ConfigurationLoader_OnConfigurationSaved;
        }

        void ConfigurationLoader_OnConfigurationSaved()
        {
            UpdateSPlusDisplayInformation();
        }

        private void ConfigurationLoader_OnConfigurationLoaded()
        {
            UpdateSPlusDisplayCount((ushort) ConfigurationLoader.Config.Displays.Count);

            UpdateSPlusDisplayInformation();
        }

        public void Initialize(int maxNumberOfDisplays)
        {
            _maxNumberOfDisplays = maxNumberOfDisplays;
        }

        public void AddDisplay(Display itemToAdd)
        {
            if (ConfigurationLoader.Config.Displays.Count <= _maxNumberOfDisplays)
            {
                ConfigurationLoader.Config.Displays.Add(itemToAdd);
                ConfigurationLoader.Config.NumberOfDisplays = ConfigurationLoader.Config.Displays.Count;
            }

            ConfigurationLoader.ConfigChanged = true;

            UpdateSPlusDisplayCount( (ushort)ConfigurationLoader.Config.Displays.Count);
            UpdateSPlusDisplayInformation();
        }

        public void RemoveDisplay(ushort index)
        {
            ConfigurationLoader.Config.Displays.RemoveAt(index - 1);
            ConfigurationLoader.Config.NumberOfDisplays = ConfigurationLoader.Config.Displays.Count;

            ConfigurationLoader.ConfigChanged = true;

            UpdateSPlusDisplayCount((ushort) ConfigurationLoader.Config.Displays.Count);
            UpdateSPlusDisplayInformation();

        }

        public void UpdateDisplay(ushort index, Display itemToUpdate)
        {
            ConfigurationLoader.Config.Displays[index - 1] = itemToUpdate;

            ConfigurationLoader.ConfigChanged = true;

            UpdateSPlusDisplayInformation();
        }

        public void SaveDisplayConfiguration()
        {
            ConfigurationLoader.SaveConfigurationFile(ConfigurationLoader.FileName);
        }

        public void GetDisplay(ushort index)
        {
            if (UpdateSelectedDisplay != null)
            {
                UpdateSelectedDisplay(ConfigurationLoader.Config.Displays[index -1]);
            }
        }

        private void UpdateSPlusDisplayCount(ushort count)
        {
            if (UpdateDisplayCount != null)
            {
                UpdateDisplayCount(count);
            }
        }

        private void UpdateSPlusDisplayInformation()
        {
            for (var i = 0; i < ConfigurationLoader.Config.Displays.Count; i++)
            {
                if (UpdateDisplayInformation != null)
                {
                    UpdateDisplayInformation((ushort)(i + 1), ConfigurationLoader.Config.Displays[i]);
                }
            }
        }
    }
}