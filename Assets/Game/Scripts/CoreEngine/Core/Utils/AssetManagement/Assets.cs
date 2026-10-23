using System;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using Object = UnityEngine.Object;

namespace Core.Utils.AssetManagement
{
    public static class Assets
    {
        public const string DefaultPath = "Assets";
        private static bool LogEnabled => false;
        private static readonly string[] PathBuffer = new string[1];

        private static string[] FindAssetsGuids<T>(string path)
        {
            return FindAssetsGuids(path, typeof(T));
        }

        private static string[] FindAssetsGuids(string path, System.Type type)
        {
#if UNITY_EDITOR
            string full = type.FullName.Replace("UnityEngine.", string.Empty);

            int index = path.IndexOf("/Assets", StringComparison.Ordinal);
            if (index >= 0)
                path = path.Remove(0, index + 1);

            if (LogEnabled)
                Debug.Log($"Searching {full} in {path}...");
            PathBuffer[0] = path;
            return AssetDatabase.FindAssets($"t:{full}", PathBuffer);
#else
            return System.Array.Empty<string>();
#endif
        }

        public static HashSet<TObject> Find<TObject>(HashSet<TObject> output, string path = DefaultPath)
            where TObject : Object
        {
#if UNITY_EDITOR
            string[] guids = FindAssetsGuids<TObject>(path);
            int duplicates = 0;
            foreach (string guid in guids)
            {
                var assets = AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(guid), typeof(Object)) as TObject;
                if (!output.Add(assets))
                    duplicates++;
            }

            if (LogEnabled)
                Debug.Log($"{(duplicates > 0 ? $"Duplicates: {duplicates}\n" : string.Empty)} Found unique: {output.Count}");
            return output;
#else
            return null;
#endif
        }

        public static IEnumerator FindRoutine<TObject>(HashSet<TObject> output, System.Action<float> onProgressUpdated, string path = DefaultPath, int yieldEvery = 5)
            where TObject : Object
        {
#if UNITY_EDITOR
            string[] guids = FindAssetsGuids<TObject>(path);
            int duplicates = 0;
            float i = 0;
            foreach (string guid in guids)
            {
                var assets = AssetDatabase.LoadAssetAtPath<TObject>(AssetDatabase.GUIDToAssetPath(guid));
                if (!output.Add(assets))
                    duplicates++;

                if (i % yieldEvery != 0)
                    continue;

                onProgressUpdated.Invoke(i / guids.Length);
                yield return null;
            }

            onProgressUpdated.Invoke(1);
            Debug.Log($"{(duplicates > 0 ? $"Duplicates: {duplicates}\n" : string.Empty)} Found unique: {output.Count}");
#else
            yield return null;
#endif
        }

        public static TObject FindSingleObject<TObject>(string path = DefaultPath)
            where TObject : Object
        {
#if UNITY_EDITOR
            foreach (string guid in FindAssetsGuids<TObject>(path))
            {
                var asset = AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(guid), typeof(Object)) as TObject;
                return asset;
            }
#endif
            return null;
        }

        public static HashSet<T> Find<T>(string path = DefaultPath)
            where T : Object
        {
            var assetsFound = new HashSet<T>();
            Find(assetsFound, path);
            return assetsFound;
        }

        public static HashSet<TTarget> FindObjects<TBase, TTarget>(HashSet<TTarget> assetsFound, HashSet<TBase> assetsBase, string path = DefaultPath)
            where TBase : Object
        {
            foreach (TBase baseObj in Find(assetsBase, path))
            {
                if (baseObj is TTarget target)
                    assetsFound.Add(target);
            }

            return assetsFound;
        }

        public static HashSet<TTarget> FindObjects<TBase, TTarget>(string path = DefaultPath)
            where TBase : Object
        {
            return FindObjects(new HashSet<TTarget>(), new HashSet<TBase>(), path);
        }

        public static HashSet<T> FindInGameObjects<T>(HashSet<T> assetsFound, string path = DefaultPath)
            where T : class
        {
            foreach (GameObject gameObject in Find<GameObject>(path))
            foreach (T component in gameObject.GetComponentsInChildren<T>(true))
                assetsFound.Add(component);
            return assetsFound;
        }

        public static HashSet<T> FindInGameObjects<T>(string path = DefaultPath)
            where T : class
        {
            return FindInGameObjects(new HashSet<T>(), path);
        }

        [System.Flags]
        public enum ObjFlags
        {
            GameObject = 1 << 0,
            ScriptableObject = 1 << 1
        }

        public static HashSet<T> FindInterface<T>(ObjFlags flags = ObjFlags.GameObject | ObjFlags.ScriptableObject, string path = DefaultPath)
            where T : class
        {
            var assetsFound = new HashSet<T>();
            if ((flags & ObjFlags.GameObject) != 0)
            {
                foreach (T component in FindInGameObjects<T>(path))
                    assetsFound.Add(component);
            }

            if ((flags & ObjFlags.ScriptableObject) != 0)
            {
                foreach (ScriptableObject scriptableObject in Find<ScriptableObject>(path))
                    if (scriptableObject is T t)
                        assetsFound.Add(t);
            }

            return assetsFound;
        }
    }
}