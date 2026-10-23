using UnityEngine;

namespace Utils.Extensions
{
    public static class ComponentExtensions
    {
        public static void GetComponents<T>(this Component monoBehaviour, out T[] value)
        {
            value = monoBehaviour.GetComponents<T>();
        }

        public static void GetComponentsInChildren<T>(this Component monoBehaviour, out T[] value, bool includeInactive = true)
        {
            value = monoBehaviour.GetComponentsInChildren<T>(includeInactive);
        }

        public static void GetComponentInChildren<T>(this Component monoBehaviour, out T value)
        {
            value = monoBehaviour.GetComponentInChildren<T>();
        }

        public static bool TryGetComponentInChildren<T>(this Component transform, in string name, out T target)
            where T : Component
        {
            Transform child = transform.FindInChildren(name);
            if (child)
                return child.TryGetComponent(out target);

            target = null;
            return false;
        }

        public static Transform FindInChildren(this Component component, in string name)
        {
            Transform obj = component.transform.Find(name);
            if (obj)
                return obj;

            foreach (Transform child in component.transform)
            {
                obj = child.FindInChildren(name);
                if (obj)
                    return obj;
            }

            return null;
        }

        public static T GetComponentInParent<T>(this Component component, bool includeInactive)
            where T : class
        {
            if (includeInactive)
            {
                Transform parent = component.transform.parent;
                while (parent)
                {
                    if (parent.TryGetComponent(out T target))
                        return target;

                    if (parent.parent != null)
                        parent = parent.parent;
                }

                return null;
            }

            return component.GetComponentInParent<T>();
        }
    }
}