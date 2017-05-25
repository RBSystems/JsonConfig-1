using System;

namespace SC.SimplSharp.Config
{
    public interface ISettingsWriter
    {
        /// <summary>
        /// Method to save the entire configuration file. The type must be a class and must have a public default constructor.
        /// </summary>
        /// <typeparam name="T">Type to serialize</typeparam>
        /// <param name="settings">Object to save</param>
        void Save<T>(T settings) where T : class, new();

        /// <summary>
        /// Method to save a secton of the configuration file. The type must be a class and must have a public default constructor.
        /// </summary>
        /// <typeparam name="T">Type to serialize</typeparam>
        /// <param name="settings">Object to save</param>
        void SaveSection<T>(T settings) where T : class, new();

        /// <summary>
        /// Method to save the specified object to a section of the JSON file. The type must be a class and must have a public default constructor.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="settings"></param>
        /// <param name="sectionName"></param>
        void SaveSection<T>(T settings, string sectionName) where T : class, new();

        /// <summary>
        /// Method to save the entire configuration.
        /// </summary>
        /// <param name="type">Type to serialize from</param>
        /// <param name="settings">object to save</param>
        void Save(Type type, object settings);

        /// <summary>
        /// Method to save a section of the configuration
        /// </summary>
        /// <param name="type">Type to serialize from</param>
        /// <param name="settings">object to save</param>
        void SaveSection(Type type, object settings);

        void SaveSection(Type type, object settings, string sectionName);
    }
}