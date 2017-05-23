using Crestron.SimplSharp.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SC.SimplSharp.Utilities
{
    public abstract class ListConfigurationUpdaterBase
    {
        public delegate void UpdateCountDelegate(ushort count);

        protected int MaxArrayLength;

        protected ListConfigurationUpdaterBase()
        {
            ConfigurationLoader.OnConfigurationLoaded += ConfigurationLoader_OnConfigurationLoaded;
            ConfigurationLoader.OnConfigurationSaved += ConfigurationLoader_OnConfigurationSaved;
        }

        public UpdateCountDelegate UpdateCount { get; set; }
         
        protected static JArray Info { get; set; }

        protected virtual void ConfigurationLoader_OnConfigurationSaved()
        {
            UpdateSPlusInformation();
        }

        public virtual void SetMaxCount(int maxLength)
        {
            MaxArrayLength = maxLength;
        }

        protected abstract void UpdateSPlusInformation();

        protected virtual void ConfigurationLoader_OnConfigurationLoaded()
        {
            UpdateSplusCount((ushort) Info.Count);
            UpdateSPlusInformation();
        }

        protected virtual void UpdateSplusCount(ushort count)
        {
            if (UpdateCount != null)
            {
                UpdateCount(count);
            }
        }

        protected virtual void AddToArray<T>(T objectToAdd)
        {
            if (Info.Count >= MaxArrayLength) return;

            var tempJo = JObject.FromObject(objectToAdd);

            Info.Add(tempJo);

            UpdateSplusCount((ushort) Info.Count);
        }

        protected virtual void RemoveFromArray(int index)
        {
            Info.RemoveAt(index - 1);

            UpdateSplusCount((ushort) Info.Count);
        }

        protected virtual void UpdateEntry<T>(int index, T objectToUpdate)
        {
            Info[index - 1] = JObject.FromObject(objectToUpdate);
        }

        protected static void InitializeInfo(object o)
        {
            if (Info == null)
            {
                Info = JArray.FromObject(o);
            }
        }

        protected static T SaveChanges<T>()
        {
            var serializer = new JsonSerializer();

            ConfigurationLoader.ConfigChanged = true;

            return (T) serializer.Deserialize(new JTokenReader(Info), typeof (T));
        }

        protected static T GetObject<T>(JToken jo)
        {
            var serializer = new JsonSerializer();

            return
                (T) serializer.Deserialize(new JTokenReader(jo), typeof (T).GetCType());
        }
    }
}