using UnityEngine;

namespace Dythervin.ObjectPool.Component
{
    public interface IComponentPool : IObjectPool
    {
    }

    public interface IComponentPoolOut<out T> : IComponentPool, IObjectPoolOut<T>
        where T : class
    {
        T Get(bool setActive = true);
    }

    public interface IComponentPool<T> : IComponentPoolOut<T>, IObjectPool<T>
        where T : UnityEngine.Component
    {
    }
}