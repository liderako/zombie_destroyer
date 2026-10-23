namespace Dythervin.UpdateSystem.Main
{
    public interface IUpdater
    {
        void Update(float deltaTime);
    }

    public interface IUpdaterExt : IUpdater
    {
    }
}