namespace SCConfigSplus.JSON
{
    /// <summary>
    /// Class for describing a source in a system. Contains information on the input, name, whether the source is shareable over VTC, and whether the source should route on sync detect.
    ///  </summary>
    public class Source
    {
        /// <summary>
        /// Input. This is the input on the switcher where the source is connected.
        /// </summary>
        public ushort Input { get; set; }

        /// <summary>
        /// Enabled. If set to 1, the source can be routed.
        /// </summary>
        public ushort Enabled { get; set; }

        /// <summary>
        /// Name. This is the name to be displayed on the UI.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Shareable Source. If this is true, the source can also be selected when in a Video Conference.
        /// </summary>
        public ushort ShareableSource { get; set; }

        /// <summary>
        /// Enable Sync Detection. If true, this source can be automatically routed when sync is detected.
        /// </summary>
        public ushort EnableSyncDetect { get; set; }
    }
}