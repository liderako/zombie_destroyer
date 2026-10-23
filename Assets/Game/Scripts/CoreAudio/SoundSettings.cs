using UnityEngine;

namespace CoreAudio
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Settings/SoundSettings", fileName = "SoundSettings")]
    public class SoundSettings : ScriptableObject
    {
        [Header("UI")]
        public SFXObject clickByAnyButton;
        public SFXObject winScreenAppear;
        public SFXObject loseScreenAppear;
    }
}