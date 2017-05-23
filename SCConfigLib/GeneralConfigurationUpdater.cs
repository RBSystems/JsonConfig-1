using System;
using Crestron.SimplSharp;
using Newtonsoft.Json.Linq;

namespace SC.SimplSharp.Utilities
{
    public class EnvironmentConfigurationUpdater : BasicConfigurationUpdaterBase
    {
        public delegate void UpdateEnvironmentInformationDelegate(
            ushort lightingEnabled, ushort shadesEnabled, ushort autoswitchEnabled);

        public UpdateEnvironmentInformationDelegate UpdateEnvironmentInformation { get; set; }

        protected override void ConfigurationLoader_OnConfigurationSaved()
        {
            Info = JObject.FromObject(ConfigurationLoader.Config.Environment);

            base.ConfigurationLoader_OnConfigurationSaved();
        }

        protected override void ConfigurationLoader_OnConfigurationLoaded()
        {
            Info = JObject.FromObject(ConfigurationLoader.Config.Environment);

            base.ConfigurationLoader_OnConfigurationLoaded();
        }

        public override void SetStringValue(string property, string value)
        {
            try
            {
                InitializeInfo(ConfigurationLoader.Config.Environment);

                Info[property] = value;

                ConfigurationLoader.Config.Environment = SaveChanges<Environment>();
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
                InitializeInfo(ConfigurationLoader.Config.Environment);

                Info[property] = value > 0;

                ConfigurationLoader.Config.Environment = SaveChanges<Environment>();
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
                InitializeInfo(ConfigurationLoader.Config.Environment);

                Info[property] = value;

                ConfigurationLoader.Config.Environment = SaveChanges<Environment>();
            }
            catch (Exception ex)
            {
                ErrorLog.Error("Error converting value: {0} Exception: {1}", property, ex);
            }
        }

        protected override void UpdateSplusInformation()
        {
            var handler = UpdateEnvironmentInformation;

            var lightingEnabled = (bool) Info["Lighting"] ? (ushort) 1 : (ushort) 0;
            var shadesEnabled = (bool)Info["Shades"] ? (ushort)1 : (ushort)0;
            var autoswitchEnabled = (bool)Info["AutoSwitch"] ? (ushort)1 : (ushort)0;
            if (handler != null)
            {
                handler(lightingEnabled, shadesEnabled, autoswitchEnabled);
            }
        }
    }
}