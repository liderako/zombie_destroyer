#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

#if UNITY_EDITOR
public class SortChildrenByLocalY : MonoBehaviour
{
    [MenuItem("Custom/Sort Children by Local Y (Descending)")]
    static void SortChildren()
    {
        Transform selectedTransform = Selection.activeTransform;

        if (selectedTransform != null)
        {
            Transform[] children = new Transform[selectedTransform.childCount];
            for (int i = 0; i < selectedTransform.childCount; i++)
            {
                children[i] = selectedTransform.GetChild(i);
            }
            System.Array.Sort(children, (a, b) => b.localPosition.y.CompareTo(a.localPosition.y));
            for (int i = 0; i < children.Length; i++)
            {
                children[i].SetSiblingIndex(i);
            }
        }
        else
        {
            Debug.LogWarning("Select a parent object with child objects to sort.");
        }
    }
}
#endif
