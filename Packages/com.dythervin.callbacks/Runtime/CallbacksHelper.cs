using Dythervin.Callbacks.Inner;
using UnityEngine;

namespace Dythervin.Callbacks
{
    public static class CallbacksHelper
    {
        public static ICallbacks GetCallbacks(this Component component)
        {
            return GetCallbacks(component.gameObject);
        }

        public static ICallbacks GetCallbacks(this GameObject gameObject)
        {
            return GetCallbacks<ICallbacks, Inner.Callbacks>(gameObject);
        }

        public static IParticleCallbacks GetParticleCallbacks(this Component component)
        {
            return GetParticleCallbacks(component.gameObject);
        }

        public static IParticleCallbacks GetParticleCallbacks(this GameObject gameObject)
        {
            return GetCallbacks<IParticleCallbacks, ParticleCallbacks>(gameObject);
        }

        public static ICollisionCallbacks GetCollisionCallbacks(this Component component)
        {
            return GetCollisionCallbacks(component.gameObject);
        }

        public static ICollisionCallbacks GetCollisionCallbacks(this GameObject gameObject)
        {
            return GetCallbacks<ICollisionCallbacks, CollisionCallbacks>(gameObject);
        }

        public static ICollisionStayCallback GetCollisionStayCallback(this Component component)
        {
            return GetCollisionStayCallback(component.gameObject);
        }

        public static ICollisionStayCallback GetCollisionStayCallback(this GameObject gameObject)
        {
            return GetCallbacks<ICollisionStayCallback, CollisionStayCallback>(gameObject);
        }

        public static ITriggerCallbacks GetTriggerCallbacks(this Component component)
        {
            return GetTriggerCallbacks(component.gameObject);
        }

        public static ITriggerCallbacks GetTriggerCallbacks(this GameObject gameObject)
        {
            return GetCallbacks<ITriggerCallbacks, TriggerCallbacks>(gameObject);
        }

        public static ITriggerStayCallback GetTriggerStayCallback(this Component component)
        {
            return GetTriggerStayCallback(component.gameObject);
        }

        public static ITriggerStayCallback GetTriggerStayCallback(this GameObject gameObject)
        {
            return GetCallbacks<ITriggerStayCallback, TriggerStayCallback>(gameObject);
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private static TInterface GetCallbacks<TInterface, TComponent>(GameObject gameObject)
            where TComponent : Component, TInterface
        {
            return gameObject.TryGetComponent(out TInterface disableListener) ? disableListener : gameObject.AddComponent<TComponent>();
        }

        private static bool DestroyIfEmpty(ICallbackBase callbackBase)
        {
            if (callbackBase.IsEmpty && callbackBase is Object obj)
            {
                Object.Destroy(obj);
                return true;
            }

            return false;
        }
    }
}