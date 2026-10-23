using UnityEngine;

namespace Dythervin.Callbacks
{
    public interface IGameObjectAttached
    {
        // ReSharper disable once InconsistentNaming
        public GameObject gameObject { get; }
    }
}