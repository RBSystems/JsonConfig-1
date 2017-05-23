using Crestron.SimplSharp;

namespace SCRoutingLib.Model
{
    public delegate void SendRouteToSwitcherDelegate(ushort input, ushort output, ushort type);

    public delegate void RouteChangedEventHandler(Route route);

    public delegate ushort GetOutputStatusDelegate(ushort output, ushort type);

    public delegate void UpdateOutputStatusTextDelegate(ushort output, SimplSharpString sourceName, ushort type);

    public delegate void SyncChangedEventHandler(ushort input, bool sync);
}