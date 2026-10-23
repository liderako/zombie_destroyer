#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif
using UnityEngine;

namespace Utils
{
    [System.Serializable]
    public struct Ref<T> : ISerializationCallbackReceiver
        where T : class
    {
#if ODIN_INSPECTOR
        [ValidateInput(nameof(Validate))]
#endif
        [SerializeField]
        private Object value;

        [System.NonSerialized]
        public T Value;

        void ISerializationCallbackReceiver.OnBeforeSerialize() { }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            Value = value as T;
        }

        private bool Validate(Object obj)
        {
            switch (obj)
            {
                case GameObject gameObject:
                {
                    if (!gameObject.TryGetComponent(out T component))
                        return false;

                    value = component as Object;
                    return true;
                }
                default:
                    return obj is T;
            }
        }

        public void SetValue(Object obj)
        {
            switch (obj)
            {
                case GameObject gameObject:
                {
                    if (!gameObject.TryGetComponent(out T component))
                        return;

                    value = component as Object;
                    break;
                }
                case T component:
                    value = component as Object;
                    break;
            }
        }

        public static implicit operator T(Ref<T> a)
        {
            return a.Value;
        }

        public static implicit operator Ref<T>(Object component)
        {
            return new Ref<T> { value = component };
        }

        public static implicit operator Ref<T>(T component)
        {
            return new Ref<T> { value = component as Component };
        }
    }
}