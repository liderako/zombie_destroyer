using UnityEngine;

namespace Dythervin.Callbacks
{
    public delegate void TriggerHandler(ITriggerCallbacks callbacks, Collider other);

    public delegate void TriggerStayHandler(ITriggerStayCallback callbacks, Collider other);

    public delegate void CollisionHandler(ICollisionCallbacks callbacks, Collision other);

    public delegate void CollisionStayHandler(ICollisionStayCallback callbacks, Collision other);

    public interface ICollisionStayCallback : ICallbackBase
    {
        public event CollisionStayHandler OnStay;
    }
}