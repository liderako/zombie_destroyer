using UnityEngine;

namespace Gameplay.Stack.Item.Utils
{
    public sealed class ItemMoverTransform : ItemMoverWorldBase
    {
        private Transform targetTransform;
        protected override Vector3 TargetPosition => targetTransform.TransformPoint(Offset);
        protected override Quaternion TargetRotation => targetTransform.rotation;
        protected override Vector3 TargetRotationEuler => TargetRotation.eulerAngles;

        public void SetTarget(Transform targetParent, in Vector3 localPosition)
        {
            if (targetParent == targetTransform && localPosition == Offset)
                return;

            targetTransform = targetParent;
            SetOffset(localPosition);
        }

        protected override void Reached()
        {
            transform.parent = targetTransform;
            base.Reached();
        }
    }
}