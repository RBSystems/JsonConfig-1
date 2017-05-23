namespace SC.SimplSharp.Utilities
{
    public class GeneralConfigurationUpdater
    {
        public delegate void UpdateLightingInformationDelegate(ushort enabled);
        public delegate void UpdateShadesInformationDelegate(ushort enabled);
        public delegate void UpdateAutoswitchInformationDelegate(ushort enabled);

        public UpdateAutoswitchInformationDelegate UpdateAutoSwitchInformation {get; set;}
        public UpdateShadesInformationDelegate UpdateShadesInformation { get; set; }
        public UpdateLightingInformationDelegate UpdateLightingInformation { get; set; }

        public GeneralConfigurationUpdater()
        {
            ConfigurationLoader.OnConfigurationLoaded += ConfigurationLoader_OnConfigurationLoaded;
            ConfigurationLoader.OnConfigurationSaved += ConfigurationLoader_OnConfigurationSaved;
        }

        void ConfigurationLoader_OnConfigurationSaved()
        {
            UpdateSPlusInformation();
        }

        void ConfigurationLoader_OnConfigurationLoaded()
        {
            UpdateSPlusInformation();
        }

        public void UpdateLightingSetting(ushort value)
        {
            if (ConfigurationLoader.Config == null) return;

            var currentValue = ConfigurationLoader.Config.Lighting ? 1 : 0;

            if (value == currentValue) return;

            ConfigurationLoader.Config.Lighting = value > 0;

            ConfigurationLoader.ConfigChanged = true;
        }

        public void UpdateShadesSetting(ushort value)
        {
            if (ConfigurationLoader.Config == null) return;

            var currentValue = ConfigurationLoader.Config.Shades ? 1 : 0;

            if (value == currentValue) return;

            ConfigurationLoader.Config.Shades = value > 0;

            ConfigurationLoader.ConfigChanged = true;
        }

        public void UpdateAutoSwitchSetting(ushort value)
        {
            if (ConfigurationLoader.Config == null) return;

            var currentValue = ConfigurationLoader.Config.AutoSwitch ? 1 : 0;

            if (value == currentValue) return;

            ConfigurationLoader.Config.AutoSwitch = value > 0;

            ConfigurationLoader.ConfigChanged = true;
        }

        private void UpdateSPlusInformation()
        {
            if (UpdateAutoSwitchInformation != null)
            {
                ushort value = ConfigurationLoader.Config.AutoSwitch ? (ushort) 1 : (ushort) 0;

                UpdateAutoSwitchInformation(value);
            }

            if (UpdateShadesInformation != null)
            {
                ushort value = ConfigurationLoader.Config.Shades ? (ushort)1 : (ushort)0;

                UpdateShadesInformation(value);
            }

            if (UpdateLightingInformation != null)
            {
                ushort value = ConfigurationLoader.Config.Lighting ? (ushort)1 : (ushort)0;

                UpdateLightingInformation(value);
            }
        }
    }
}