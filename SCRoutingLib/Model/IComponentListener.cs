using SCRoutingLib.Model;

namespace SCRoutingLib
{
    public interface IComponentListener
    {
        ushort ComponentListenerId { get; }

        event RouteChangedEventHandler RouteChanged;
        event SyncChangedEventHandler SyncChanged;

        void Route(Route routeToExecute);

        void Refresh();
    }
}