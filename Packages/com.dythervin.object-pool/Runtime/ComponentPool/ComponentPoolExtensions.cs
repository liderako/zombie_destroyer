using UnityEngine;

namespace Dythervin.ObjectPool.Component
{
    public static class ComponentPoolExtensions
    {
        public static T Get<T>(this IComponentPool<T> pool, Transform parent, bool setActive = true)
            where T : UnityEngine.Component
        {
            T obj = pool.Get(setActive);
            obj.transform.SetParent(parent);
            return obj;
        }

        public static T Get<T>(this IComponentPool<T> pool, in Vector3? position = null, in Quaternion? rotation = null,
            bool setActive = true, Space space = Space.World)
            where T : UnityEngine.Component
        {
            T obj = pool.Get(setActive);
            if (position.HasValue || rotation.HasValue)
            {
                SetPositionAndRotation(obj.transform, position, rotation, space);
            }

            return obj;
        }

        public static T Get<T>(this IComponentPool<T> pool, Transform parent, in Vector3 position, in Quaternion? rotation = null,
            Space space = Space.World, bool setActive = true)
            where T : UnityEngine.Component
        {
            T obj = pool.Get(parent, setActive);
            Transform objTransform = obj.transform;
            objTransform.SetParent(parent, true);
            SetPositionAndRotation(objTransform, position, rotation, space);

            return obj;
        }

        private static void SetPositionAndRotation(Transform objTransform, in Vector3? position, in Quaternion? rotation, Space space)
        {
            if (position == null && rotation == null)
                return;

            if (position != null && rotation != null)
            {
                if (space == Space.World)
                {
                    objTransform.SetPositionAndRotation(position.Value, rotation.Value);
                }
                else
                {
                    objTransform.localRotation = rotation.Value;
                    objTransform.localPosition = position.Value;
                }
            }
            else if (position != null)
            {
                if (space == Space.World)
                    objTransform.position = position.Value;
                else
                    objTransform.localPosition = position.Value;
            }
            else
            {
                if (space == Space.World)
                    objTransform.rotation = rotation.Value;
                else
                    objTransform.localRotation = rotation.Value;
            }
        }
    }
}