using System;
using Crestron.SimplSharp;

namespace SC.SimplSharp.Utilities
{
    public class SwitcherConfigurationUpdater
    {
        public delegate void UpdateSwitcherInformationDelegate(Switcher matrixSwitch);

        public UpdateSwitcherInformationDelegate UpdateSwitcherInformation { get; set; }

        public SwitcherConfigurationUpdater()
        {
            ConfigurationLoader.OnConfigurationLoaded += ConfigurationLoader_OnConfigurationLoaded;
            ConfigurationLoader.OnConfigurationSaved += ConfigurationLoader_OnConfigurationSaved;
        }

        void ConfigurationLoader_OnConfigurationSaved()
        {
            UpdateSplusSwitcherInformation();
        }

        void ConfigurationLoader_OnConfigurationLoaded()
        {
            UpdateSplusSwitcherInformation();
        }

        public void UpdateSwitcherType(ushort value)
        {
            if (ConfigurationLoader.Config == null) return;

            if (value == ConfigurationLoader.Config.Switcher.Type) return;

            ConfigurationLoader.Config.Switcher.Type = value;

            ConfigurationLoader.ConfigChanged = true;
        }

        public void UpdateSwitcherIpAddress(string value)
        {
            if (ConfigurationLoader.Config == null) return;

            if (value == ConfigurationLoader.Config.Switcher.IpAddress) return;

            ConfigurationLoader.Config.Switcher.IpAddress = value;

            if (ConfigurationLoader.Config.Switcher.Type == 1)
            {
                var consoleResponse = String.Empty;

                CrestronConsole.SendControlSystemCommand(String.Format("addp 50 {0} -p:{1}",value,InitialParametersClass.ApplicationNumber), ref consoleResponse);

                CrestronConsole.PrintLine(String.Format("IP Table Updated: {0}",consoleResponse));
            }

            ConfigurationLoader.ConfigChanged = true;
            ConfigurationLoader.RebootRequired = true;
        }

        private void UpdateSplusSwitcherInformation()
        {
            if (UpdateSwitcherInformation != null)
            {
                UpdateSwitcherInformation(ConfigurationLoader.Config.Switcher);
            }
        }
    }
}