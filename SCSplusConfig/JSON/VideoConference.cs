namespace SCConfigSplus.JSON
{
    /// <summary>
    /// Class for containing information about a Video Conference unit. Contains information on Type, communications, inputs, and outputs.
    /// </summary>
    public class VideoConference
    {
        /// <summary>
        /// If true, the room has Video Conferencing.
        /// </summary>
        public bool Enable { get; set; }

        /// <summary>
        /// Type of Video Conference Unit.
        /// </summary>
        public ushort Type { get; set; }

        /// <summary>
        /// Communications Type. Could be RS-232, TCP/IP.
        /// </summary>
        public ushort CommunicationsType { get; set; }

        /// <summary>
        /// The output on the Switcher that shareable content will be routed to.
        /// </summary>
        public ushort ContentOutput { get; set; }

        /// <summary>
        /// The output on the switcher that cameras will be routed to, if cameras are to be routed through the switcher
        /// </summary>
        public ushort CameraOutput { get; set; }

        public ushort NumberOfMonitors { get; set; }

        /// <summary>
        /// Input on the switcher that the primary monitor output is connected to.
        /// </summary>
        public ushort Monitor1Input { get; set; }

        /// <summary>
        /// Input on the switcher that the secondary monitor output is connected to.
        /// </summary>
        public ushort Monitor2Input { get; set; }
        public string IpAddress { get; set; }//
        public ushort Port { get; set; }//
    }
}