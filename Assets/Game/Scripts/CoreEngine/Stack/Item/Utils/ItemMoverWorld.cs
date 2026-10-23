using UnityEngine;

namespace Gameplay.Stack.Item.Utils
{
    public class ItemMoverWorld : ItemMoverWorldBase
    {
        private Quaternion targetRotation;
        protected override Vector3 TargetPosition => Offset;
        protected override Quaternion TargetRotation => targetRotation;
        protected override Vector3 TargetRotationEuler => targetRotation.eulerAngles;
        
        public void SetTarget(in Vector3 position, in Quaternion rotation)
        {
            targetRotation = rotation;
            SetOffset(position);
        }
        
        public void SetTarget(in Vector3 position)
        {
            targetRotation = Quaternion.identity;
            SetOffset(position);
        }

    }
}