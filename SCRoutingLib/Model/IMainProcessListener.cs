namespace SCRoutingLib
{
    public interface IMainProcessListener:IMainProcess
    {
        bool AudioFollowsVideo { get; set; }

        void RegisterComponentListener(IComponentListener requester);

        void Refresh();
    }
}