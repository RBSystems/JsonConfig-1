using Crestron.SimplSharp.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SC.SimplSharp.Utilities
{
    public abstract class BasicConfigurationUpdaterBase
    {
        protected ushort MaxArrayLength;

        protected BasicConfigurationUpdaterBase()
        {
            ConfigurationLoader.OnConfigurationLoaded += ConfigurationLoader_OnConfigurationLoaded;
            ConfigurationLoader.OnConfigurationSaved += ConfigurationLoader_OnConfigurationSaved;
        }

        protected static JObject Info { get; set; }

        public abstract void SetStringValue(string property, string value);
        public abstract void SetBoolValue(string property, ushort value);
        public abstract void SetUshortValue(string property, ushort value);

        protected virtual void ConfigurationLoader_OnConfigurationSaved()
        {
            UpdateSplusInformation();
        }

        protected virtual void ConfigurationLoader_OnConfigurationLoaded()
        {
            UpdateSplusInformation();
        }

        protected abstract void UpdateSplusInformation();

        protected static void InitializeInfo(object o)
        {
            if (Info == null)
            {
                Info = JObject.FromObject(o);
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