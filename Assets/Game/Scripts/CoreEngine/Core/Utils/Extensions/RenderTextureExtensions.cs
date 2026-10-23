using UnityEngine;

namespace _GameData_.Scripts.Core.Utils.Extensions
{
    public static class RenderTextureExtensions
    {
        public static void ClearOutRenderTexture(this RenderTexture renderTexture)
        {
            RenderTexture rt = RenderTexture.active;
            RenderTexture.active = renderTexture;
            GL.Clear(true, true, Color.clear);
            RenderTexture.active = rt;
        }
    }
}