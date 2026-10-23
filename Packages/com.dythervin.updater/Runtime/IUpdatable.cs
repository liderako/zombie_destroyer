namespace Dythervin.UpdateSystem
{
    public interface IUpdatable : IUpdatableBase
    {
        void OnUpdate();
    }

    public interface IUpdatableInterval : IUpdatableBase
    {
        public float Interval { get; }

        float UntilUpdate { get; set; }

        void OnUpdate();
    }
}