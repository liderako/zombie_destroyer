using UnityEngine;

namespace PencilFlow.Configs
{
    [CreateAssetMenu(fileName = "PunchBounceSettings", menuName = "ScriptableObjects/Settings/PunchBounceSettings", order = 0)]
    public class PunchBounceSettings : ScriptableObject
    {
        public Vector3 punchScale = new Vector3(0.1f, 0.1f, 0.1f);
        public float durationPunch = 0.1f;
        public int vibrationPunch = 10;
    }
}