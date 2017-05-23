// For Basic SIMPL# Classes

namespace SCRoutingLib
{
    public abstract class Component : IMainProcessListener
    {
        protected IComponentListener Listener;
        public int Id { get; set; }

        public ushort RouteProcessorId { get; set; }

        public abstract bool AudioFollowsVideo { get; set; }

        public virtual void RegisterComponentListener(IComponentListener requester)
        {
            if (requester == Listener) return;

            Listener.RouteChanged -= Listener_RouteChanged;
            Listener.SyncChanged -= Listener_SyncChanged;

            Listener = requester;

            Listener.RouteChanged += Listener_RouteChanged;
            Listener.SyncChanged += Listener_SyncChanged;
        }

        protected abstract void Listener_RouteChanged(Route route);
        protected abstract void Listener_SyncChanged(ushort input, bool sync);

        public abstract void Refresh();

        public abstract bool AllowAudioBreakaway { get; set; }
    }
}