using System.Collections.Generic;
using System.Linq;
using Crestron.SimplSharp;
using SC.SimplSharp.Utilities;
using SCRoutingLib.Model;

namespace SCRoutingLib
{
    public class SourceSelector : Component
    {
        public UpdateOutputStatusTextDelegate UpdateOutputStatusText { get; set; }

        public override bool AudioFollowsVideo { get; set; }
        public override bool AllowAudioBreakaway { get; set; }


        public SourceSelector()
        {
            ConfigurationLoader.OnConfigurationLoaded += ConfigurationLoader_OnConfigurationLoaded;
            ConfigurationLoader.OnConfigurationSaved += ConfigurationLoader_OnConfigurationSaved;
        }

        private void ConfigurationLoader_OnConfigurationSaved()
        {
            RouteProcessorId = ConfigurationLoader.Config.Switcher.Type;

            RegisterMe();
        }

        private void ConfigurationLoader_OnConfigurationLoaded()
        {
            RouteProcessorId = ConfigurationLoader.Config.Switcher.Type;

            RegisterMe();
        }

        public override void Refresh()
        {
            Listener.Refresh();
        }

        public override void RegisterComponentListener(IComponentListener requester)
        {
            Listener.RouteChanged -= Listener_RouteChanged;
            Listener.SyncChanged -= Listener_SyncChanged;

            base.RegisterComponentListener(requester);

            Listener.RouteChanged += Listener_RouteChanged;
            Listener.SyncChanged += Listener_SyncChanged;
        }

        private void Listener_SyncChanged(ushort input, bool sync)
        {
            
        }

        private void Listener_RouteChanged(Route route)
        {
            var handler = UpdateOutputStatusText;

            if (handler == null) return;

            var name = ConfigurationLoader.Config.Sources.First(s => s.Input == route.Input).Name;

            handler(route.Output, new SimplSharpString(name), (ushort) route.Type);
        }

        public void MappedRoute(ushort source, ushort destination, ushort type)
        {
            if (ConfigurationLoader.Config == null) return;

            var input = ConfigurationLoader.Config.Sources[source].Input;
            var output = ConfigurationLoader.Config.Displays[destination].Output;

            Route(input, output, type);
        }

        public void Route(ushort input, ushort output, ushort type)
        {
            Listener.Route(new Route {Input = input, Output = output, Type = (RouteType) type});
        }

        private void RegisterMe()
        {
            RegistrationUtil.Register(this);
        }
    }
}