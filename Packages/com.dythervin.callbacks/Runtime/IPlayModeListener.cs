namespace Dythervin.Callbacks
{
    public interface IPlayModeListener
    {
#if UNITY_2021_3_OR_NEWER
        protected internal
#endif
        bool MainThreadOnly { get; }
#if UNITY_2021_3_OR_NEWER
        protected internal
#endif
        void OnEnterPlayMode();
    }
}