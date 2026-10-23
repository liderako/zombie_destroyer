using UnityEngine;

namespace Utils.Extensions
{
    public static class GameObjectExtensions
    {
        public static void SetLayerRecursively(this GameObject gameObject, int layer)
        {
            gameObject.layer = layer;
            foreach (Transform child in gameObject.transform)
                SetLayerRecursively(child.gameObject, layer);
        }

        public static void SetTagRecursively(this GameObject gameObject, string tag)
        {
            gameObject.tag = tag;
            foreach (Transform child in gameObject.transform)
                SetTagRecursively(child.gameObject, tag);
        }
    }
}