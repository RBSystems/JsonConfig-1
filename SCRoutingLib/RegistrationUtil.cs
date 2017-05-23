using System;
using System.Collections.Generic;
using Crestron.SimplSharp;

namespace SCRoutingLib
{
    public class RegistrationUtil
    {
        private const ushort RegistrationTimeout = 2500;
        private static ushort _sourceSelectorId;
        private static readonly List<Component> Components = new List<Component>();
        private static CTimer _registrationTimer;
        private static readonly CEvent AllowRegistration = new CEvent(false,false);

        public static event EventHandler ComponentsChanged;

        public static void Register(Component me)
        {
            if (Components.Contains(me))
            {
                var index = Components.FindIndex(c => c == me);

                Components[index] = me;

                var handler = ComponentsChanged;

                if (handler != null)
                {
                    handler(null,new EventArgs());
                }

                return;
            }
                
            ++_sourceSelectorId;
            me.Id = _sourceSelectorId;
            Components.Add(me);

            if (_registrationTimer == null)
            {
                _registrationTimer = new CTimer(o => AllowRegistration.Set(), RegistrationTimeout);
            }
            else
            {
                _registrationTimer.Reset(RegistrationTimeout);
            }
        }

        public static List<Component> GetComponentsList(ushort routeListenerId)
        {
            return Components.FindAll(x => x.RouteProcessorId == routeListenerId);
        }

        public static void RegisterRouteListener(IComponentListener listener)
        {
            try
            {
                AllowRegistration.Wait();
                foreach (var router in GetComponentsList(listener.ComponentListenerId))
                {
                    router.RegisterComponentListener(listener);
                }
            }
            finally
            {
                _registrationTimer = null;
            }
        }

    }
}