using System.Collections.Generic;
using UnityEngine;

namespace Utils.Extensions
{
    public static class Vector3Extensions
    {
        public static Transform GetClosest(this in Vector3 point, IEnumerable<Transform> targets)
        {
            Transform closest = null;
            float minDist = float.MaxValue;
            foreach (Transform target in targets)
            {
                float dist = (target.position - point).sqrMagnitude;
                if (dist < minDist)
                {
                    closest = target;
                    minDist = dist;
                }
            }

            return closest;
        }

        public static Transform GetFarthest(this in Vector3 point, IEnumerable<Transform> targets)
        {
            Transform farthest = null;
            float maxDist = float.MinValue;
            foreach (Transform target in targets)
            {
                float dist = (target.position - point).sqrMagnitude;
                if (dist > maxDist)
                {
                    farthest = target;
                    maxDist = dist;
                }
            }

            return farthest;
        }
        public static Vector3 GetClosest(this in Vector3 point, IEnumerable<Vector3> targets)
        {
            Vector3 closest = Vector3.positiveInfinity;
            float minDist = float.MaxValue;
            foreach (Vector3 target in targets)
            {
                float dist = (target - point).sqrMagnitude;
                if (dist < minDist)
                {
                    closest = target;
                    minDist = dist;
                }
            }

            return closest;
        }

        public static Vector3 GetFarthest(this in Vector3 point, IEnumerable<Vector3> targets)
        {
            Vector3 farthest = Vector3.zero;
            float maxDist = float.MinValue;
            foreach (Vector3 target in targets)
            {
                float dist = (target - point).sqrMagnitude;
                if (dist > maxDist)
                {
                    farthest = target;
                    maxDist = dist;
                }
            }

            return farthest;
        }

        public static void GetClosest(this in Vector3 point, IEnumerable<Transform> targets, out Transform closest)
        {
            closest = GetClosest(point, targets);
        }
        public static void GetFarthest(this in Vector3 point, IEnumerable<Transform> targets, out Transform closest)
        {
            closest = GetFarthest(point, targets);
        }
    }
}