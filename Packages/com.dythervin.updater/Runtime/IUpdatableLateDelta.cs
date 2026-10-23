namespace Dythervin.UpdateSystem
{
    public interface IUpdatableLateDelta : IUpdatableBase
    {
        void OnUpdate(float deltaTime);
    }
}