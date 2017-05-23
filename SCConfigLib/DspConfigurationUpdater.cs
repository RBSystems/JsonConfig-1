namespace SC.SimplSharp.Utilities
{
    public class DspConfigurationUpdater
    {
        public delegate void UpdateDspInformationDelegate(Dsp information, ushort audioConferencingValue);

        public UpdateDspInformationDelegate UpdateDspInformation { get; set; }

        public DspConfigurationUpdater()
        {
            ConfigurationLoader.OnConfigurationLoaded += ConfigurationLoader_OnConfigurationLoaded;
            ConfigurationLoader.OnConfigurationSaved += ConfigurationLoader_OnConfigurationSaved;
        }

        void ConfigurationLoader_OnConfigurationSaved()
        {
            UpdateSplusDspInformation();
        }

        void ConfigurationLoader_OnConfigurationLoaded()
        {
            UpdateSplusDspInformation();
        }

        public void UpdateDspType(ushort value)
        {
            if (ConfigurationLoader.Config == null) return;

            if (value == ConfigurationLoader.Config.Dsp.Type) return;

            ConfigurationLoader.Config.Dsp.Type = value;

            ConfigurationLoader.ConfigChanged = true;
        }

        public void UpdateDspCommunicationsType(ushort value)
        {
            if (ConfigurationLoader.Config == null) return;

            if (value == ConfigurationLoader.Config.Dsp.CommunicationsType) return;

            ConfigurationLoader.Config.Dsp.CommunicationsType = value;

            ConfigurationLoader.ConfigChanged = true;
        }

        public void UpdateDspPort(ushort value)
        {
            if (ConfigurationLoader.Config == null) return;

            if (value == ConfigurationLoader.Config.Dsp.Port) return;
                
            ConfigurationLoader.Config.Dsp.Port = value;
            
            ConfigurationLoader.ConfigChanged = true;
        }

        public void UpdateDspIpAddress(string value)
        {
            if (ConfigurationLoader.Config == null) return;

            if (value == ConfigurationLoader.Config.Dsp.IpAddress) return;
            
            ConfigurationLoader.Config.Dsp.IpAddress = value;
            
            ConfigurationLoader.ConfigChanged = true;
        }

        public void UpdateAudioConferencing(ushort value)
        {
            if (ConfigurationLoader.Config == null) return;

            var currentValue = ConfigurationLoader.Config.Features.AudioConference ? 1 : 0;

            if (value == currentValue) return;

            ConfigurationLoader.Config.Features.AudioConference = value > 0;

            ConfigurationLoader.ConfigChanged = true;
        }

        private void UpdateSplusDspInformation()
        {
            if (UpdateDspInformation != null)
            {
                UpdateDspInformation(ConfigurationLoader.Config.Dsp, ConfigurationLoader.Config.Features.AudioConference ? (ushort) 1 : (ushort) 0);
            }
        }
    }
}