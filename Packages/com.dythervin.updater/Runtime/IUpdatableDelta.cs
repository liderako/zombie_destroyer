namespace Dythervin.UpdateSystem
{
    public interface IUpdatableDelta : IUpdatableBase
    {
        void OnUpdate(float deltaTime);
    }
}