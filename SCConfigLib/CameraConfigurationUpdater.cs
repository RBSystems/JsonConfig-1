using System;
using System.Collections.Generic;
using Crestron.SimplSharp;
using Newtonsoft.Json.Linq;

namespace SC.SimplSharp.Utilities
{
    public class CameraConfigurationUpdater : ListConfigurationUpdaterBase
    {
        public delegate void UpdateCameraInformationDelegate(ushort index, Camera item);

        public delegate void UpdateSelectedItemDelegate(Camera item);

        public CameraConfigurationUpdater()
        {
            MaxArrayLength = 2;
        }

        public UpdateSelectedItemDelegate UpdateSelectedItem { get; set; }
        public UpdateCameraInformationDelegate UpdateCameraInformation { get; set; }

        protected override void ConfigurationLoader_OnConfigurationLoaded()
        {
            Info = JArray.FromObject(ConfigurationLoader.Config.Cameras);

            base.ConfigurationLoader_OnConfigurationLoaded();
        }

        protected override void ConfigurationLoader_OnConfigurationSaved()
        {
            Info = JArray.FromObject(ConfigurationLoader.Config.Cameras);

            base.ConfigurationLoader_OnConfigurationSaved();
        }

        public void AddCamera(Camera itemToAdd)
        {
            try
            {
                InitializeInfo(ConfigurationLoader.Config.Cameras);

                AddToArray(itemToAdd);

                ConfigurationLoader.Config.Cameras = SaveChanges<List<Camera>>();
                ConfigurationLoader.Config.NumberOfCameras = Info.Count;

                UpdateSPlusInformation();
            }
            catch (Exception ex)
            {
                ErrorLog.Error("Error adding camera. {0}", ex);
            }
        }

        public void RemoveCamera(ushort index)
        {
            try
            {
                InitializeInfo(ConfigurationLoader.Config.Cameras);

                RemoveFromArray(index);

                ConfigurationLoader.Config.Cameras = SaveChanges<List<Camera>>();
                ConfigurationLoader.Config.NumberOfCameras = Info.Count;

                UpdateSPlusInformation();
            }
            catch (Exception ex)
            {
                ErrorLog.Error("Error removing camera. {0}", ex);
            }
        }

        public void UpdateCamera(ushort index, Camera itemToUpdate)
        {
            try
            {
                InitializeInfo(ConfigurationLoader.Config.Cameras);

                UpdateEntry(index, itemToUpdate);

                ConfigurationLoader.Config.Cameras = SaveChanges<List<Camera>>();
                ConfigurationLoader.Config.NumberOfCameras = Info.Count;

                UpdateSPlusInformation();
            }
            catch (Exception ex)
            {
                ErrorLog.Error("Error updating camera. {0}", ex);
            }
        }

        public void GetCamera(ushort index)
        {
            if (UpdateSelectedItem != null)
            {
                UpdateSelectedItem(GetObject<Camera>(Info[index - 1]));
            }
        }

        protected override void UpdateSPlusInformation()
        {
            if (UpdateCameraInformation == null) return;

            for (ushort i = 0; i < Info.Count; i++)
            {
                UpdateCameraInformation((ushort) (i + 1), GetObject<Camera>(Info[i]));
            }
        }
    }
}