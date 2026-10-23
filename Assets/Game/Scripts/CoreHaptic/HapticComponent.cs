using Lofelt.NiceVibrations;
using UnityEngine;

namespace Haptic.Components
{
    public class HapticComponent : MonoBehaviour
    {
        private bool vibrationAvailable = true;
        private Coroutine hapticSequenceCoroutine;
        private const string VIBRATION_SAVE_LOAD_PATH = "Vibration_Enabled";
        
        private void Awake() 
        {
            LoadVibration();
        }
        
        public void LightImpact()
        {
            if (!vibrationAvailable)
                return;
#if NICE_VIBRATION
            HapticPatterns.PlayPreset(HapticPatterns.PresetType.SoftImpact);
#endif
        }

        public void HeavyImpact()
        {
            if (!vibrationAvailable)
                return;
#if NICE_VIBRATION
            HapticPatterns.PlayPreset(HapticPatterns.PresetType.HeavyImpact);
#endif
        }

        public void MediumImpact()
        {
            if (!vibrationAvailable)
                return;
#if NICE_VIBRATION
            HapticPatterns.PlayPreset(HapticPatterns.PresetType.LightImpact);
#endif
        }

        public void WarningImpact()
        {
            if (!vibrationAvailable)
                 return;
#if NICE_VIBRATION
            HapticPatterns.PlayPreset(HapticPatterns.PresetType.Warning);
#endif
        }
        
        public bool VibrationAvailable
        {
            get => vibrationAvailable;
            set => ChangeVibration(value);
        }

        private void LoadVibration()
        {
            int vibration = PlayerPrefs.GetInt(VIBRATION_SAVE_LOAD_PATH, 1);
            vibrationAvailable = vibration == 1;
        }

        private void ChangeVibration(bool canVibrate)
        {
            int targetVibration = (canVibrate) ? 1 : 0;
            PlayerPrefs.SetInt(VIBRATION_SAVE_LOAD_PATH, targetVibration);

            vibrationAvailable = canVibrate;
        }
    }
}