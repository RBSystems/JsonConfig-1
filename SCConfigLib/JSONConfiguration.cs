using System.Collections.Generic;

namespace SC.SimplSharp.Utilities
{
    public class Environment
    {
        public bool Shades { get; set; }
        public bool Lighting { get; set; }
        public bool AutoSwitch { get; set; }
    }

    public class Display
    {
        public string Name { get; set; }
        public ushort Manufacturer { get; set; }
        public ushort CommunicationsType { get; set; }
        public string IpAddress { get; set; }
        public ushort Port { get; set; }
        public ushort Output { get; set; }
    }

    public class Source
    {
        public ushort Input { get; set; }
        public ushort Enabled { get; set; }
        public string Name { get; set; }
        public string ShareableSource { get; set; }
        public string EnableSyncDetect { get; set; }
    }

    public class Switcher
    {
        public ushort Type { get; set; }
        public string IpAddress { get; set; }
    }

    public class Chromebox
    {
        public bool Enable { get; set; }
        public bool HqPresent { get; set; }
        public ushort HqPresentOutput { get; set; }
        public ushort CfmInput { get; set; }
        public ushort CameraInput { get; set; }
        public string CameraControlType { get; set; }
    }

    public class VideoConference
    {
        public bool Enable { get; set; }//
        public ushort Type { get; set; }//
        public ushort CommunicationsType { get; set; }//
        public ushort ContentOutput { get; set; }
        public ushort CameraOutput { get; set; }
        public ushort NumberOfMonitors { get; set; }
        public ushort Monitor1Input { get; set; }
        public ushort Monitor2Input { get; set; }
        public string IpAddress { get; set; }//
        public ushort Port { get; set; }//
    }

    public class Camera
    {
        public ushort Manufacturer { get; set; }
        public string Name { get; set; }
        public ushort Input { get; set; }
        public ushort ControlType { get; set; }
        public string ControlIpAddress { get; set; }
        public ushort ControlPort { get; set; }
    }

    public class Dsp
    {
        public ushort Type { get; set; }
        public ushort CommunicationsType { get; set; }
        public string IpAddress { get; set; }
        public ushort Port { get; set; }
        public bool AudioConference { get; set; }
    }

    public class Configuration
    {
        public int NumberOfDisplays { get; set; }
        public int NumberOfInputs { get; set; }
        public int NumberOfCameras { get; set; }
        public List<Display> Displays { get; set; }
        public List<Source> Sources { get; set; }
        public Switcher Switcher { get; set; }
        public Dsp Dsp { get; set; }
        public VideoConference VideoConference { get; set; }
        public List<Camera> Cameras { get; set; } 
        public Environment Environment { get; set; }

        public Configuration()
        {
            Environment = new Environment();
            Displays = new List<Display>();
            Sources = new List<Source>();
            Cameras = new List<Camera>();
            VideoConference = new VideoConference();
            Switcher = new Switcher();
            Dsp = new Dsp();
        }
    }


}