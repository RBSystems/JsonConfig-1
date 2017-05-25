namespace SCConfigSplus.JSON
{
    /// <summary>
    /// Class for describing a camera. Contains information about the manufacture, method of control, and where the device is connected to the system.
    /// </summary>
    public class Camera
    {
        /// <summary>
        /// Type. This corresponds to the Camera manufacturer
        /// </summary>
        public ushort Type { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Input
        /// </summary>
        public ushort Input { get; set; }

        /// <summary>
        /// ControlType. Could be RS232, TCP/IP, or through the VTC Codec.
        /// </summary>
        public ushort ControlType { get; set; }

        /// <summary>
        /// IP Address
        /// </summary>
        public string IpAddress { get; set; }

        /// <summary>
        /// TCP port for control
        /// </summary>
        public ushort Port { get; set; }
    }
}