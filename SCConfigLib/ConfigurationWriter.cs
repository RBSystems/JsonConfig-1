namespace SC.SimplSharp.Config
{
    public class ConfigurationWriter<T> where T : class, new()
    {
        private readonly ISettingsWriter _settingsWriter;
        
        /// <summary>
        /// Initializes ConfigurationWriter with a writer that save the settings to a file
        /// </summary>
        /// <param name="writer"></param>
        public ConfigurationWriter(ISettingsWriter writer)
        {
            _settingsWriter = writer;
        }

        /// <summary>
        /// Save the specified object to the file
        /// </summary>
        /// <param name="settings">Settings to save</param>
        public void SaveSettings(T settings)
        {
            _settingsWriter.SaveSection(settings);
        }
    }
}