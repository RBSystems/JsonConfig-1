using SCRoutingLib.Model;

namespace SCRoutingLib
{
    public class Route
    {
        public ushort Input { get; set; }
        public ushort Output { get; set; }
        public RouteType Type { get; set; }
    }
}