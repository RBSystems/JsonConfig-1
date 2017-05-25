namespace SCConfigSplus.JSON
{
    /// <summary>
    /// Class for containing information about a Switcher. Contains Type, IP Address, and if the Sync detection is enabled.
    /// </summary>
    public class Switcher
    {
        /// <summary>
        /// Type. Sets the type of switcher, typically a DM switcher.
        /// </summary>
        public ushort Type { get; set; }

        /// <summary>
        /// IP Address for communication with the Switcher. Only used for HD-MD series switchers.
        /// </summary>
        public string IpAddress { get; set; }

        /// <summary>
        /// Enable Sync Detection. This is the global control for sync detection.
        /// If this is true, a source with its EnableSyncDetection also high will be automatically routed when sync is detected.
        /// If this is false, no sources will be automatically routed.
        /// </summary>
        public bool EnableSyncDetection { get; set; }
    }
}