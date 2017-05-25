using System;
using Crestron.SimplSharp;
using Crestron.SimplSharp.CrestronIO;
using Crestron.SimplSharp.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SC.SimplSharp.Config;
using Activator = Crestron.SimplSharp.Reflection.Activator;

namespace SCConfigSplus.Readers
{
    public class JsonSettingsReader:ISettingsReader
    {
        private readonly string _configurationFilePath;
        private static readonly CCriticalSection FileReadCriticalSection = new CCriticalSection();

        private static readonly JsonSerializerSettings JsonSerializerSettings = new JsonSerializerSettings
        {
            ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };

        /// <summary>
        /// Initialize the reader and sets the path to the file to write.
        /// </summary>
        /// <param name="configurationFilePath">Path to file to write</param>
        public JsonSettingsReader(string configurationFilePath)
        {
            _configurationFilePath = configurationFilePath;
        }

        /// <summary>
        /// Loads a JSON-formatted file. 
        /// </summary>
        /// <typeparam name="T">Type to return. Must be a class and have a public default constructor</typeparam>
        /// <returns>Object of specified type</returns>
        public T Load<T>() where T : class, new()
        {
            return Load(typeof(T)) as T;   
        }

        /// <summary>
        /// Loads a section of a JSON-formatted file.
        /// </summary>
        /// <typeparam name="T">Type to return. Must be a class and have a public default constructor</typeparam>
        /// <returns>Object of specified type</returns>
        public T LoadSection<T>() where T : class, new()
        {
            return LoadSection(typeof(T)) as T;
        }

        /// <summary>
        /// Loads a section of a JSON-formatted file.
        /// </summary>
        /// <typeparam name="T">Type to return. Must be a class and have a public default constructor</typeparam>
        /// <param name="sectionName">Name of the section to return</param>
        /// <returns>Object of specified type</returns>
        public T LoadSection<T>(string sectionName) where T : class, new()
        {
            return LoadSection(typeof (T),sectionName) as T;
        }

        /// <summary>
        /// Loads a object from a JSON-formatted file.
        /// </summary>
        /// <param name="type">Type to return.</param>
        /// <returns>
        /// Returns an object containing the data from the file.
        /// If the file doesn't exist an empty instance of the object is returned.
        /// </returns>
        public object Load(Type type)
        {
            if (!File.Exists(_configurationFilePath))
            {
                return Activator.CreateInstance(type.GetCType());
            }

            var json = ReadFile();

            return JsonConvert.DeserializeObject(json, type, JsonSerializerSettings);
        }

        /// <summary>
        /// Load a section of a JSON-Formatted file
        /// </summary>
        /// <param name="type">Type to return</param>
        /// <returns>
        /// Returns an object containing the requested data.
        /// If the file doesn't exist, doesn't contain any data, or doesn't contain the requested section, an empty object is returned.
        /// </returns>
        public object LoadSection(Type type)
        {
            return LoadSection(type, type.Name);
        }

        /// <summary>
        /// Load a section of a JSON-Formatted file
        /// </summary>
        /// <param name="type">Type to return</param>
        /// <param name="sectionName">Property to return</param>
        /// <returns>
        /// Returns on object containing the requested datat.
        /// If the file doesn't exist, doesn't contain any data, or doesn't contain the requested section, an empty object is returned
        /// </returns>
        public object LoadSection(Type type, string sectionName)
        {
            if (!File.Exists(_configurationFilePath) || new FileInfo(_configurationFilePath).Length == 0)
            {
                return Activator.CreateInstance(type.GetCType());
            }

            var json = ReadFile();


            var settingsSection = GetSectionJToken(json, sectionName);

            return settingsSection == null
                ? Activator.CreateInstance(type.GetCType())
                : JsonConvert.DeserializeObject(settingsSection.ToString(), type, JsonSerializerSettings);
        }

        private string ReadFile()
        {
            string json;

            try
            {
                FileReadCriticalSection.Enter();
                using (var reader = new StreamReader(_configurationFilePath))
                {
                    json = reader.ReadToEnd();
                }
            }
            finally
            {
                FileReadCriticalSection.Leave();
            }
            return json;
        }

        /// <summary>
        /// Method to convert the specified section of the JSON string to a JToken
        /// </summary>
        /// <param name="json">string to convert</param>
        /// <param name="section">section name to convert</param>
        /// <returns>JToken containing the desired section</returns>
        private JToken GetSectionJToken(string json, string section)
        {
            var settingsData = new JObject();

            try
            {
                settingsData = JObject.Parse(json);
            }
            catch (JsonReaderException ex)
            {
                ErrorLog.Error("Exception parsing JSON at line {0} position {1}. {2}", ex.LineNumber, ex.LinePosition,
                    ex.Message);
            }

            var settingsSection = settingsData[section];

            return settingsSection;
        }
    }
}