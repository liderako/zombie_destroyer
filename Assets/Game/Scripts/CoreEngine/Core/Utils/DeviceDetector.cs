using UnityEngine;

namespace _GameData_.Scripts.Core.Utils
{
    public class DeviceDetector
    {
        public static bool IsTablet { get; private set; }
        
        public static void UpdateStatus()
        {
#if UNITY_EDITOR
            IsTablet = true;
#elif UNITY_ANDROID
            string model = SystemInfo.deviceModel.ToLower();

            // Check for common tablet keywords
            if (model.Contains("tab") || model.Contains("table"))
            {
                IsTablet = true;
                return;
            }
            
            float dpi = Screen.dpi;
            float width = Screen.width / dpi;
            float height = Screen.height / dpi;
            float diagonalInches = Mathf.Sqrt(width * width + height * height);

            // Typically, devices with a screen size greater than 6.5 inches are considered tablets.
            IsTablet =  diagonalInches >= 7f;

#elif UNITY_IOS
        string model = SystemInfo.deviceModel.ToLower();
        if (model.Contains("ipad"))
        {
            IsTablet = true;
        }
#else
            IsTablet = false;
#endif
        }
    }
}