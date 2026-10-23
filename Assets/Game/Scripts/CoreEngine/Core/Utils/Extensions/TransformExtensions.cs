using System.Collections.Generic;
using UnityEngine;

namespace Utils.Extensions
{
    public static class TransformExtensions
    {
        public static void GetClosest(this Transform point, IEnumerable<Transform> targets, out Transform closest)
        {
            closest = point.position.GetClosest(targets);
        }

        public static Transform GetClosest(this Transform point, IEnumerable<Transform> targets)
        {
            return point.position.GetClosest(targets);
        }

        public static void GetFarthest(this Transform point, IEnumerable<Transform> targets, out Transform closest)
        {
            closest = point.position.GetFarthest(targets);
        }

        public static Transform GetFarthest(this Transform point, IEnumerable<Transform> targets)
        {
            return point.position.GetFarthest(targets);
        }


        private static readonly Stack<Transform> Buffer = new Stack<Transform>(128);

        public static int ChildCount(this Transform transform)
        {
            Buffer.Clear();
            int amount = 1;
            Buffer.Push(transform);

            do
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    Transform child = transform.GetChild(i);
                    if (child.childCount > 0)
                        Buffer.Push(child);
                    amount++;
                }
            } while (Buffer.TryPop(out transform));


            return amount;
        }

        public static void GetNameValues(this Transform transform, Dictionary<string, Transform> dictionary)
        {
            Buffer.Clear();
            Buffer.Push(transform);

            do
            {
                dictionary.Add(transform.name, transform);
                for (int i = 0; i < transform.childCount; i++)
                {
                    Buffer.Push(transform.GetChild(i));
                }
            } while (Buffer.TryPop(out transform));
        }

        public static void GetNameValues(this Transform transform, Dictionary<int, Transform> dictionary)
        {
            Buffer.Clear();
            Buffer.Push(transform);

            do
            {
                dictionary.Add(transform.name.GetHashCode(), transform);
                for (int i = 0; i < transform.childCount; i++)
                {
                    Buffer.Push(transform.GetChild(i));
                }
            } while (Buffer.TryPop(out transform));
        }
        
        public static void GetChildComponentsRecursively<T>(this Transform parent, List<T> components, int depth, int maxDepth=5)
        {
            if (depth >= maxDepth)
                return;
            foreach (Transform child in parent)
            {
                T value = child.GetComponent<T>();
                if (value != null)
                {
                    components.Add(value);
                }
                GetChildComponentsRecursively(child, components, depth + 1);
            }
        }
    }
}