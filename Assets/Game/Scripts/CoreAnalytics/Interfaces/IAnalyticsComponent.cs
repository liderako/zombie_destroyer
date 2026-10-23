namespace CoreGameplay.Components.Interfaces
{
    public interface IAnalyticsComponent
    {
        public void SendLevelStartedEvent(int levelNumber);

        public void SendLevelCompletedEvent(bool win);
    }
}