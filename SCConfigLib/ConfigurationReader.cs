namespace SC.SimplSharp.Config
{
    /// <summary>
    /// 
    /// </summary>
    public class ConfigurationReader<T> where T: class, new()
    {
        private readonly ISettingsReader _settingsReader;
        private T _settings;

        /// <summary>
        /// Initializes ConfigurationReader with a reader that will load the settings from a file.
        /// </summary>
        /// <param name="reader">ISettingsReader</param>
        public ConfigurationReader(ISettingsReader reader)
        {
            _settingsReader = reader;
        }

        /// <summary>
        /// Method to read the settings from the specified section.
        /// </summary>
        /// <returns>Object of type T containing the values read from the file</returns>
        public T ReadSettings()
        {
            _settings = _settingsReader.LoadSection<T>();

            return _settings;
        }

        public T ReadSettings(string sectionName)
        {
            _settings = _settingsReader.LoadSection<T>(sectionName);

            return _settings;
        }
    }
}