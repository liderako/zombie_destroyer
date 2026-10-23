namespace Dythervin.UpdateSystem
{
    public interface IUpdatableFixedDelta : IUpdatableBase
    {
        void OnUpdate(float deltaTime);
    }
}