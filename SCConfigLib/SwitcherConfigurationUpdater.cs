using System;
using Crestron.SimplSharp;
using Newtonsoft.Json.Linq;

namespace SC.SimplSharp.Utilities
{
    public class SwitcherConfigurationUpdater:BasicConfigurationUpdaterBase
    {
        public delegate void UpdateSwitcherInformationDelegate(Switcher information);

        public UpdateSwitcherInformationDelegate UpdateSwitcherInformation { get; set; }

        protected override void ConfigurationLoader_OnConfigurationSaved()
        {
            Info = JObject.FromObject(ConfigurationLoader.Config.Switcher);

            base.ConfigurationLoader_OnConfigurationSaved();
        }

        protected override void ConfigurationLoader_OnConfigurationLoaded()
        {
            Info = JObject.FromObject(ConfigurationLoader.Config.Switcher);

            base.ConfigurationLoader_OnConfigurationLoaded();
        }

        public override void SetStringValue(string property, string value)
        {
            try
            {
                InitializeInfo(ConfigurationLoader.Config.Switcher);

                Info[property] = value;

                ConfigurationLoader.Config.Switcher = SaveChanges<Switcher>();
            }
            catch (Exception ex)
            {
                ErrorLog.Error("Error converting value: {0} Exception: {1}", property, ex);
            }
        }

        public override void SetBoolValue(string property, ushort value)
        {
            try
            {
                InitializeInfo(ConfigurationLoader.Config.Switcher);

                Info[property] = value > 0;

                ConfigurationLoader.Config.Switcher = SaveChanges<Switcher>();
            }
            catch (Exception ex)
            {
                ErrorLog.Error("Error converting value: {0} Exception: {1}", property, ex);
            }
        }

        public override void SetUshortValue(string property, ushort value)
        {
            try
            {
                InitializeInfo(ConfigurationLoader.Config.Switcher);

                Info[property] = value;

                ConfigurationLoader.Config.Switcher = SaveChanges<Switcher>();
            }
            catch (Exception ex)
            {
                ErrorLog.Error("Error converting value: {0} Exception: {1}", property, ex);
            }
        }

        protected override void UpdateSplusInformation()
        {
            var handler = UpdateSwitcherInformation;

            if (handler != null)
            {
                handler(GetObject<Switcher>(Info));
            }
        }
    }
}