#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Utils.Extensions
{
    public static class RectTransformExtensions
    {
        [MenuItem("CONTEXT/RectTransform/Anchor2Edges")]
        private static void SetAnchors(MenuCommand command)
        {
            var target = (RectTransform)command.context;
            target.SetAnchors();
        }

        public static void SetAnchors(this RectTransform target)
        {
            if (target.parent == null || !(target.parent is RectTransform parent))
                return;

            Undo.RecordObject(target, nameof(SetAnchors));
            Rect parentSize = parent.rect;
            
            if (target.anchorMax != Vector2.one || target.anchorMin != Vector2.zero)
            {
                target.SetAnchorsFull(parent);
            }

            Vector2 min = target.offsetMin;
            Vector2 max = target.offsetMax * -1;
            target.anchorMin = new Vector2(min.x / parentSize.width, min.y / parentSize.height);
            target.anchorMax = new Vector2(1 - max.x / parentSize.width, 1 - max.y / parentSize.height);
            target.offsetMax = Vector2.zero;
            target.offsetMin = Vector2.zero;
            target.Dirty();
        }

        public static void SetAnchorsFull(this RectTransform target, RectTransform parent)
        {
            Vector3 position = target.position;
            target.SetParent(null);
            target.anchorMin = Vector2.zero;
            target.anchorMax = Vector2.one;
            target.SetParent(parent);
            target.position = position;
            target.Dirty();
        }
    }
}
#endif