using UnityEngine;
using UnityEngine.Audio;

namespace CoreAudio
{
    public class AudioGameService : MonoBehaviour
    {
        [SerializeField] private AudioMixer mixer;

        private AudioListener audioListener;
        
        public bool IsSoundEnable { get; private set; }
        public bool IsMusicEnable { get; private set; }

        private const string sound_enabled_key = "sound_enabled";
        private const string music_enabled_key = "music_enabled";
        
        private void Start()
        {
            Load();
        }
        
        private void Load()
        {
            IsSoundEnable = PlayerPrefs.GetInt(sound_enabled_key, 1) == 1;
            IsMusicEnable = PlayerPrefs.GetInt(music_enabled_key, 1) == 1;
            ChangeSoundStatus(IsSoundEnable);
            ChangeMusicStatus(IsMusicEnable);
        }

        public void ChangeSoundStatus(bool status)
        {
            PlayerPrefs.SetInt(sound_enabled_key, (status) ? 1 : 0);
            OnHandleSoundStatusAction(status);
        }
        
        public void ChangeMusicStatus(bool status)
        {
            PlayerPrefs.SetInt(music_enabled_key, (status) ? 1 : 0);
            OnHandleAudioStatusAction(status);
        }

        private void OnHandleAudioStatusAction(bool state)
        {
            if (!state)
            {
                mixer.SetFloat("MusicVolume", -80f);
            }
            else
            {
                mixer.SetFloat("MusicVolume", 0);
            }
        }
        
        private void OnHandleSoundStatusAction(bool state)
        {
            if (!state)
            {
                mixer.SetFloat("SFXVolume", -80f);
            }
            else
            {
                mixer.SetFloat("SFXVolume", 0);
            }
        }
    }
}