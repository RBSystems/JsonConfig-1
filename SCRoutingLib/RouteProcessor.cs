using System;
using Crestron.SimplSharp;
using SCRoutingLib.Model;

namespace SCRoutingLib.Switches
{
    public class RouteProcessor:IComponentListener,IMainProcess
    {
        private ushort _maxInput;
        private ushort _maxOutput;
        private const ushort MinOutput = 1;

        public RouteProcessor()
        {
            RegistrationUtil.ComponentsChanged += RegistrationUtil_ComponentsChanged;
        }

        void RegistrationUtil_ComponentsChanged(object sender, EventArgs e)
        {
            RegisterMe();
        }

        public bool SwitchOffline { get; private set; }

        public ushort ComponentListenerId { get; private set; }

        public SendRouteToSwitcherDelegate SendRoute { get; set; }
        public GetOutputStatusDelegate GetOutputStatus { get; set; }

        public event RouteChangedEventHandler RouteChanged;
        public event SyncChangedEventHandler SyncChanged;

        public bool AllowAudioBreakaway { get; private set; }

        public void ProcessFeedback(ushort input, ushort output, ushort type)
        {
            var handler = RouteChanged;

            if (handler != null)
            {
                handler(new Route {Input = input, Output = output, Type = (RouteType) type});
            }
        }

        public void Route(Route routeToExecute)
        {
            if (routeToExecute.Input > _maxInput)
            {
                throw new ArgumentOutOfRangeException("routeToExecute.Input",
                    String.Format("Input must be between 0 and {0}", _maxInput));
            }

            if (routeToExecute.Output > _maxOutput || routeToExecute.Output < MinOutput)
            {
                throw new ArgumentOutOfRangeException("routeToExecute.Output",
                    String.Format("Output must be between 1 and {0}", _maxOutput));
            }

            if (SwitchOffline)
            {
                ErrorLog.Error("Switch offline. Check connections");
            }

            var handler = SendRoute;

            if (handler == null)
            {
                ErrorLog.Error("No delegates registered for SendRoute.");
                return;
            }

            handler(routeToExecute.Input, routeToExecute.Output, (ushort) routeToExecute.Type);
        }

        public void Refresh()
        {
            var delegateHandler = GetOutputStatus;
            var eventHandler = RouteChanged;

            if (eventHandler == null || delegateHandler == null) return;

            for (ushort i = 1; i <= _maxOutput; i++)
            {
                if (!AllowAudioBreakaway)
                {
                    var input = delegateHandler(i, (ushort)RouteType.Video);
                    eventHandler(new Route {Input = input, Output = i, Type = RouteType.Both});
                }
                else
                {
                    var input = delegateHandler(i, (ushort) RouteType.Video);

                    eventHandler(new Route {Input = input, Output = i, Type = RouteType.Video});

                    input = delegateHandler(i, (ushort) RouteType.Audio);

                    eventHandler(new Route {Input = input, Output = i, Type = RouteType.Audio});
                }
            }
        }

        public void SetSwitchStatus(ushort status)
        {
            SwitchOffline = (status == 1);
        }

        public void Configure(ushort id, ushort maxInput, ushort maxOutput, ushort allowAudioBreakaway)
        {
            ComponentListenerId = id;
            AllowAudioBreakaway = (allowAudioBreakaway == 1);
            _maxInput = maxInput;
            _maxOutput = maxOutput;
            RegisterMe();
        }

        public void SyncDetected(ushort input, ushort syncStatus)
        {
            var handler = SyncChanged;

            if (handler == null) return;

            handler(input, syncStatus == 1);
        }

        private void RegisterMe()
        {
            RegistrationUtil.RegisterRouteListener(this);
        }
    }
}