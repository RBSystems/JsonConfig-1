using System;
using System.Collections.Generic;
using Crestron.SimplSharp;
using Newtonsoft.Json.Linq;

namespace SC.SimplSharp.Utilities
{
    public class SourceConfigurationUpdater: ListConfigurationUpdaterBase
    {
        public delegate void UpdateSourceInformationDelegate(ushort index, Source item);

        public delegate void UpdateSelectedItemDelegate(Source item);

        public SourceConfigurationUpdater()
        {
            MaxArrayLength = 16;
        }

        public UpdateSelectedItemDelegate UpdateSelectedItem { get; set; }
        public UpdateSourceInformationDelegate UpdateSourceInformation { get; set; }

        public void Initialize(ushort maxSources)
        {
            MaxArrayLength = maxSources;
        }

        protected override void ConfigurationLoader_OnConfigurationLoaded()
        {
            Info = JArray.FromObject(ConfigurationLoader.Config.Sources);

            base.ConfigurationLoader_OnConfigurationLoaded();
        }

        protected override void ConfigurationLoader_OnConfigurationSaved()
        {
            Info = JArray.FromObject(ConfigurationLoader.Config.Sources);

            base.ConfigurationLoader_OnConfigurationSaved();
        }

        public void AddSource(Source itemToAdd)
        {
            try
            {
                InitializeInfo(ConfigurationLoader.Config.Sources);

                AddToArray(itemToAdd);

                ConfigurationLoader.Config.Sources = SaveChanges<List<Source>>();
                ConfigurationLoader.Config.NumberOfInputs = Info.Count;

                UpdateSPlusInformation();
            }
            catch (Exception ex)
            {
                ErrorLog.Error("Error adding source. {0}", ex);
            }
        }

        public void RemoveSource(ushort index)
        {
            try
            {
                InitializeInfo(ConfigurationLoader.Config.Sources);

                RemoveFromArray(index);

                ConfigurationLoader.Config.Sources = SaveChanges<List<Source>>();
                ConfigurationLoader.Config.NumberOfInputs = Info.Count;

                UpdateSPlusInformation();
            }
            catch (Exception ex)
            {
                ErrorLog.Error("Error removing source. {0}", ex);
            }
        }

        public void UpdateSource(ushort index, Source itemToUpdate)
        {
            try
            {
                InitializeInfo(ConfigurationLoader.Config.Sources);

                UpdateEntry(index, itemToUpdate);

                ConfigurationLoader.Config.Sources = SaveChanges<List<Source>>();
                ConfigurationLoader.Config.NumberOfInputs = Info.Count;

                UpdateSPlusInformation();
            }
            catch (Exception ex)
            {
                ErrorLog.Error("Error updating Source. {0}", ex);
            }
        }

        public void GetSource(ushort index)
        {
            if (UpdateSelectedItem != null)
            {
                UpdateSelectedItem(GetObject<Source>(Info[index - 1]));
            }
        }

        protected override void UpdateSPlusInformation()
        {
            if (UpdateSourceInformation == null) return;

            for (ushort i = 0; i < Info.Count; i++)
            {
                UpdateSourceInformation((ushort) (i + 1), GetObject<Source>(Info[i]));
            }
        }
    }
}