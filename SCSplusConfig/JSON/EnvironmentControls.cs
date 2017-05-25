namespace SCConfigSplus.JSON
{
    /// <summary>
    /// Class for describing the room environment.
    /// </summary>
    public class EnvironmentControls
    {
        /// <summary>
        /// Shades. If value is greater than 0, the room has shade control.
        /// </summary>
        public ushort Shades { get; set; }

        /// <summary>
        /// Lighting. If value is greater than 0, the room has lighting control.
        /// </summary>
        public ushort Lighting { get; set; }
    }
}