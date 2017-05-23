using System;
using Crestron.SimplSharp;
using Newtonsoft.Json.Linq;

namespace SC.SimplSharp.Utilities
{
    public class VtcConfigurationUpdater : BasicConfigurationUpdaterBase
    {
        public delegate void UpdateVtcInformationDelegate(VideoConference information, ushort enabled);

        public UpdateVtcInformationDelegate UpdateVtcInformation { get; set; }

        protected override void ConfigurationLoader_OnConfigurationSaved()
        {
            Info = JObject.FromObject(ConfigurationLoader.Config.VideoConference);

            base.ConfigurationLoader_OnConfigurationSaved();
        }

        protected override void ConfigurationLoader_OnConfigurationLoaded()
        {
            Info = JObject.FromObject(ConfigurationLoader.Config.VideoConference);

            base.ConfigurationLoader_OnConfigurationLoaded();
        }

        public override void SetStringValue(string property, string value)
        {
            try
            {
                InitializeInfo(ConfigurationLoader.Config.VideoConference);

                Info[property] = value;

                ConfigurationLoader.Config.VideoConference = SaveChanges<VideoConference>();
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
                InitializeInfo(ConfigurationLoader.Config.VideoConference);

                Info[property] = value > 0;

                ConfigurationLoader.Config.VideoConference = SaveChanges<VideoConference>();
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
                InitializeInfo(ConfigurationLoader.Config.VideoConference);

                Info[property] = value;

                ConfigurationLoader.Config.VideoConference = SaveChanges<VideoConference>();
            }
            catch (Exception ex)
            {
                ErrorLog.Error("Error converting value: {0} Exception: {1}", property, ex);
            }
        }

        protected override void UpdateSplusInformation()
        {
            if (UpdateVtcInformation != null)
            {
                UpdateVtcInformation(GetObject<VideoConference>(Info),
                    (bool) Info["Enable"] ? (ushort) 1 : (ushort) 0);
            }
        }
    }
}