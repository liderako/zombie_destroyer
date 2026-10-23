#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Utils.Extensions
{
    public static class ObjectExtensions
    {
        public static void Dirty(this Object obj)
        {
#if UNITY_EDITOR
            EditorUtility.SetDirty(obj);
#endif
        }

        public static void Rename(this ScriptableObject obj, string newName)
        {
#if UNITY_EDITOR
            AssetDatabase.RenameAsset(AssetDatabase.GetAssetPath(obj), newName);
#endif
        }


        
        

#if UNITY_EDITOR
        [MenuItem("CONTEXT/Transform/ResetWithoutChild")]
        private static void ResetWithoutChild(MenuCommand command)
        {
            Transform target = (Transform)command.context;
            Undo.RecordObject(target, nameof(ResetWithoutChild));
            var position = target.position;
            target.position = Vector3.zero;
            for (int i = target.childCount - 1; i >= 0; i--)
            {
                target.GetChild(i).position += position;
            }

            target.Dirty();
        }
#endif
    }
}