using System;

namespace Dythervin.UpdateSystem
{
    public interface IUpdatableBase { }

    public interface IUpdatableListenerDelta
    {
        event Action<float> OnUpdate;
        bool enabled { get; set; }
    }

    public interface IUpdatableListener
    {
        event Action OnUpdate;
        bool enabled { get; set; }
    }
}