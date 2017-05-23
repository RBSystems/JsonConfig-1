namespace SC.SimplSharp.Utilities
{
    public class ChromeboxConfigurationUpdater
    {
        public delegate void UpdateChromeboxInformationDelegate(ushort enabled, ushort hqPresentation);

        public delegate void UpdateChromeboxRoutingInformationDelegate(ushort chromeboxInput, ushort hqPresentOutput, ushort cameraInput);

        public UpdateChromeboxInformationDelegate UpdateChromeboxInformation { get; set; }
        public UpdateChromeboxRoutingInformationDelegate UpdateChromeboxRoutingInformation { get; set; }

        public ChromeboxConfigurationUpdater()
        {
            ConfigurationLoader.OnConfigurationLoaded += UpdateSplusSwitcherInformation;
            ConfigurationLoader.OnConfigurationSaved += UpdateSplusSwitcherInformation;
        }

        public void EnableChromebox(ushort value)
        {
            if (ConfigurationLoader.Config == null) return;

            var currentValue = ConfigurationLoader.Config.Chromebox.Enable ? 1 : 0;

            if (value == currentValue) return;

            ConfigurationLoader.Config.Chromebox.Enable = value > 0;

            ConfigurationLoader.ConfigChanged = true;
        }

        public void SetChromeboxInput(ushort value)
        {
            if (ConfigurationLoader.Config == null) return;

            var currentValue = ConfigurationLoader.Config.Chromebox.CfmInput;

            if(value == currentValue) return;

            ConfigurationLoader.Config.Chromebox.CfmInput = value;

            ConfigurationLoader.ConfigChanged = true;
        }

        public void SetCameraInput(ushort value)
        {
            if (ConfigurationLoader.Config == null) return;

            var currentValue = ConfigurationLoader.Config.Chromebox.CameraInput;

            if(value == currentValue) return;

            ConfigurationLoader.Config.Chromebox.CameraInput = value;

            ConfigurationLoader.ConfigChanged = true;
        }

        public void EnableHqPresentation(ushort value)
        {
            if (ConfigurationLoader.Config == null) return;

            var currentValue = ConfigurationLoader.Config.Chromebox.HqPresent ? 1 : 0;

            if (value == currentValue) return;

            ConfigurationLoader.Config.Chromebox.HqPresent = value > 0;

            ConfigurationLoader.ConfigChanged = true;
        }

        public void SetHqPresentationOutput(ushort value)
        {
            if (ConfigurationLoader.Config == null) return;

            var currentValue = ConfigurationLoader.Config.Chromebox.HqPresentOutput;

            if (value == currentValue) return;

            ConfigurationLoader.Config.Chromebox.HqPresentOutput = value;

            ConfigurationLoader.ConfigChanged = true;
        }

        private void UpdateSplusSwitcherInformation()
        {
            if (UpdateChromeboxInformation != null)
            {
                var enabled = ConfigurationLoader.Config.Chromebox.Enable ? (ushort) 1 : (ushort) 0;
                var hqPresent = ConfigurationLoader.Config.Chromebox.HqPresent ? (ushort) 1 : (ushort)0;
                UpdateChromeboxInformation(enabled, hqPresent);
            }

            if (UpdateChromeboxRoutingInformation == null) return;

            UpdateChromeboxRoutingInformation(ConfigurationLoader.Config.Chromebox.CfmInput,
                ConfigurationLoader.Config.Chromebox.HqPresentOutput,
                ConfigurationLoader.Config.Chromebox.CameraInput);
        }
    }
}