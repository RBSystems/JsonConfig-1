using System;

namespace SC.SimplSharp.Config
{
    /// <summary>
    /// Interface for a Settings Reader
    /// </summary>
    public interface ISettingsReader
    {
        /// <summary>
        /// Method to load the entire settings file. The type must be a class, and must have a public default constructor.
        ///  </summary>
        /// <typeparam name="T">The type to deserialize the settings to.</typeparam>
        /// <returns>The deserialized settings object</returns>
        T Load<T>() where T : class, new();

        /// <summary>
        /// Method to load a section of the settings file. The type must be a class, and must have a public default constructor.
        /// </summary>
        /// <typeparam name="T">Type to deserialize the settings to</typeparam>
        /// <returns>The deserialized settings object</returns>
        T LoadSection<T>() where T : class, new();

        T LoadSection<T>(string sectionName) where T : class, new();

        /// <summary>
        /// Method to load the entire settings file. The type must be a class, and must have a public default constructor.
        /// </summary>
        /// <param name="type">Type to deserialize the settings to</param>
        /// <returns>The deserialized settings object</returns>
        object Load(Type type);

        /// <summary>
        /// Method to load a section of the settings file.
        /// </summary>
        /// <param name="type">Type to deserialize the settings to</param>
        /// <returns>The deserialized settings object</returns>
        object LoadSection(Type type);
    }
}