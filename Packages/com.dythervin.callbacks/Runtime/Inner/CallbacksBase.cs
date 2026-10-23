using UnityEngine;

namespace Dythervin.Callbacks.Inner
{
    internal abstract class CallbacksBase : MonoBehaviour, ICallbackBase
    {
        protected virtual void Reset()
        {
            hideFlags = HideFlags.DontSaveInEditor | HideFlags.HideInInspector;
        }

        public abstract bool IsEmpty { get; }
    }
}