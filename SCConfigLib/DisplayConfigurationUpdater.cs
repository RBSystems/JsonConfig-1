using System;
using System.Collections.Generic;
using Crestron.SimplSharp;
using Newtonsoft.Json.Linq;

namespace SC.SimplSharp.Utilities
{
    

    public class DisplayConfigurationUpdater:ListConfigurationUpdaterBase
    {
        public delegate void UpdateDisplayInformationDelegate(ushort index, Display item);

        public delegate void UpdateSelectedItemDelegate(Display item);

        public DisplayConfigurationUpdater()
        {
            MaxArrayLength = 16;
        }

        public UpdateSelectedItemDelegate UpdateSelectedItem { get; set; }
        public UpdateDisplayInformationDelegate UpdateDisplayInformation { get; set; }

        public void Initialize(ushort maxDisplays)
        {
            MaxArrayLength = maxDisplays;
        }

        protected override void ConfigurationLoader_OnConfigurationLoaded()
        {
            Info = JArray.FromObject(ConfigurationLoader.Config.Displays);

            base.ConfigurationLoader_OnConfigurationLoaded();
        }

        protected override void ConfigurationLoader_OnConfigurationSaved()
        {
            Info = JArray.FromObject(ConfigurationLoader.Config.Displays);

            base.ConfigurationLoader_OnConfigurationSaved();
        }

        public void AddDisplay(Display itemToAdd)
        {
            try
            {
                InitializeInfo(ConfigurationLoader.Config.Displays);

                AddToArray(itemToAdd);

                ConfigurationLoader.Config.Displays = SaveChanges<List<Display>>();
                ConfigurationLoader.Config.NumberOfInputs = Info.Count;

                UpdateSPlusInformation();
            }
            catch (Exception ex)
            {
                ErrorLog.Error("Error adding Display. {0}", ex);
            }
        }

        public void RemoveDisplay(ushort index)
        {
            try
            {
                InitializeInfo(ConfigurationLoader.Config.Displays);

                RemoveFromArray(index);

                ConfigurationLoader.Config.Displays = SaveChanges<List<Display>>();
                ConfigurationLoader.Config.NumberOfInputs = Info.Count;

                UpdateSPlusInformation();
            }
            catch (Exception ex)
            {
                ErrorLog.Error("Error removing Display. {0}", ex);
            }
        }

        public void UpdateDisplay(ushort index, Display itemToUpdate)
        {
            try
            {
                InitializeInfo(ConfigurationLoader.Config.Displays);

                UpdateEntry(index, itemToUpdate);

                ConfigurationLoader.Config.Displays = SaveChanges<List<Display>>();
                ConfigurationLoader.Config.NumberOfInputs = Info.Count;

                UpdateSPlusInformation();
            }
            catch (Exception ex)
            {
                ErrorLog.Error("Error updating Display. {0}", ex);
            }
        }

        public void GetDisplay(ushort index)
        {
            if (UpdateSelectedItem != null)
            {
                UpdateSelectedItem(GetObject<Display>(Info[index - 1]));
            }
        }

        protected override void UpdateSPlusInformation()
        {
            if (UpdateDisplayInformation == null) return;

            for (ushort i = 0; i < Info.Count; i++)
            {
                UpdateDisplayInformation((ushort) (i + 1), GetObject<Display>(Info[i]));
            }
        }
    }
}