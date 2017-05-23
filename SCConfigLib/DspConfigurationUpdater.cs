using System;
using Crestron.SimplSharp;
using Newtonsoft.Json.Linq;

namespace SC.SimplSharp.Utilities
{
    public class DspConfigurationUpdater:BasicConfigurationUpdaterBase
    {
        public delegate void UpdateDspInformationDelegate(Dsp information, ushort AudioConference);

        public UpdateDspInformationDelegate UpdateDspInformation { get; set; }

        protected override void ConfigurationLoader_OnConfigurationSaved()
        {
            Info = JObject.FromObject(ConfigurationLoader.Config.Dsp);

            base.ConfigurationLoader_OnConfigurationSaved();
        }

        protected override void ConfigurationLoader_OnConfigurationLoaded()
        {
            Info = JObject.FromObject(ConfigurationLoader.Config.Dsp);

            base.ConfigurationLoader_OnConfigurationLoaded();
        }

        public override void SetStringValue(string property, string value)
        {
            try
            {
                InitializeInfo(ConfigurationLoader.Config.Dsp);

                Info[property] = value;

                ConfigurationLoader.Config.Dsp = SaveChanges<Dsp>();
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
                InitializeInfo(ConfigurationLoader.Config.Dsp);

                Info[property] = value > 0;

                ConfigurationLoader.Config.Dsp = SaveChanges<Dsp>();
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
                InitializeInfo(ConfigurationLoader.Config.Dsp);

                Info[property] = value;

                ConfigurationLoader.Config.Dsp = SaveChanges<Dsp>();
            }
            catch (Exception ex)
            {
                ErrorLog.Error("Error converting value: {0} Exception: {1}", property, ex);
            }
        }

        protected override void UpdateSplusInformation()
        {
            var handler = UpdateDspInformation;

            if (handler != null)
            {
                handler(GetObject<Dsp>(Info), (bool) Info["AudioConference"] ? (ushort) 1 : (ushort) 0);
            }
        }
    }
}