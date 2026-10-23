using Game.Scripts.CoreGameplay.Components.Config;
using UnityEngine;

namespace PencilFlow.Configs
{
    [CreateAssetMenu(fileName = "LevelConfig", menuName = "ScriptableObjects/LevelConfig", order = 0)]
    public class LevelConfig : LevelItem
    {
        [Header("Level")]
        public int levelLength = 260;

        [Header("Zombies")]
        public int zombiesCount = 44;
        public float spawnStartZ = 35f;
        public float spawnEndOffset = 10f;
        public float spawnHalfWidth = 9f;
        public float minZombieSpacing = 3f;
    }
}
