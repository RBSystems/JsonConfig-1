namespace SCConfigSplus.JSON
{
    /// <summary>
    /// Class for describing a display. Contains information on type, Control, and where the device is connected to the system.
    /// </summary>
    public class Display
    {
        /// <summary>
        /// Name. Used on the UI for source routing purposes.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Type. This corresponds to the display manufacturer.
        /// </summary>
        public ushort Type { get; set; }

        /// <summary>
        /// Communications Type. Could be RS232 or TCP/IP
        /// </summary>
        public ushort CommunicationsType { get; set; }

        /// <summary>
        /// IP Address
        /// </summary>
        public string IpAddress { get; set; }

        /// <summary>
        /// TCP Port
        /// </summary>
        public ushort Port { get; set; }

        /// <summary>
        /// Output. This is the output of the switcher that the device is connected to.
        /// </summary>
        public ushort Output { get; set; }
    }
}