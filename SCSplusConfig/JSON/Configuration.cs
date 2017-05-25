using System.Collections.Generic;

namespace SCConfigSplus.JSON
{
    /// <summary>
    /// Root Configuration class for the config file. Holds references for the rest of the configuration.
    /// </summary>
    public class Configuration
    {
        /// <summary>
        /// List of Displays contained in the configuration. The maximum is dependent on the value set from S+.
        /// </summary>
        public List<Display> Displays { get; set; }

        /// <summary>
        /// List of Sources contained in the configuration. The maximum is dependent on the value set from S+.
        /// </summary>
        public List<Source> Sources { get; set; }

        /// <summary>
        /// Switcher Configuration
        /// </summary>
        public Switcher Switcher { get; set; }

        /// <summary>
        /// DSP configuration. Contains the type, communication method, and if the DSP is handling Audio Conferencing.
        /// </summary>
        public Dsp Dsp { get; set; }

        /// <summary>
        /// VTC configuraiton. Contains VTC inputs and outputs, and the number of displays expected for the VTC unit.
        /// </summary>
        public VideoConference VideoConference { get; set; }

        /// <summary>
        /// List of Cameras contained in the configuration.
        /// </summary>
        public List<Camera> Cameras { get; set; } 

        /// <summary>
        /// Environmental 
        /// </summary>
        public EnvironmentControls EnvironmentControls { get; set; }

        /// <summary>
        /// Default constructor for the Configuration class. Initializes all components to default empty configurations.
        /// </summary>
        public Configuration()
        {
            EnvironmentControls = new EnvironmentControls();
            Displays = new List<Display>();
            Sources = new List<Source>();
            Cameras = new List<Camera>();
            VideoConference = new VideoConference();
            Switcher = new Switcher();
            Dsp = new Dsp();
        }
    }
}