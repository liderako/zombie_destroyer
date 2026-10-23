#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class ScreenshotCaptureTool : MonoBehaviour
{
    private TextureFormat nonTransp = TextureFormat.RGB24;
    [SerializeField] private Camera screenshotCamera;
    [SerializeField] private Texture2D screenshot;
    [SerializeField] private RenderTexture rt;
    [SerializeField] private Vector2 screenshotSize = new Vector2(512,512);
    
    public Texture2D CaptureScreenshot()
    {
        rt = null;
        screenshotCamera.gameObject.SetActive(true);
        
        TextureFormat textForm = nonTransp;

#if UNITY_EDITOR
        Vector2 screenSize = Handles.GetMainGameViewSize();
#else
        Vector2 screenSize = new Vector2(Screen.width, Screen.height);
#endif
                
        rt = new RenderTexture((int) screenSize.x, (int) screenSize.y, 24);
        screenshotCamera.targetTexture = rt;
        screenshot = new Texture2D(rt.width, rt.height, textForm, false);
        screenshotCamera.Render();
        RenderTexture.active = screenshotCamera.targetTexture;
        
        screenshotCamera.gameObject.SetActive(false);
        
        screenshot.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        screenshotCamera.targetTexture = null;
        RenderTexture.active = null;
        
        screenshot.Apply();
        rt.Release();
        
        ResizeTool.Resize(screenshot, (int)screenshotSize.x, (int)(screenshotSize.x * screenshot.height / screenshot.width), false, FilterMode.Bilinear);

        return screenshot;
    }
}
