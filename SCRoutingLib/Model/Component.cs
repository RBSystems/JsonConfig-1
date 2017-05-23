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
            Listener = requester;
        }

        public abstract void Refresh();

        public abstract bool AllowAudioBreakaway { get; set; }
    }
}