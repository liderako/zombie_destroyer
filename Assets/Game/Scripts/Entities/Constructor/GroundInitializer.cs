using PencilFlow.Configs;
using UnityEngine;

namespace Entities.Constructor
{
    // Lays ground tiles along +Z from the car start (z = 0) and puts the finish line at the level length
    public class GroundInitializer : MonoBehaviour
    {
        [SerializeField] private Transform groundPrefab;
        [SerializeField] private Transform finishLinePrefab;
        [SerializeField] private Transform root;
        [SerializeField] private float tileLength = 75f;
        [SerializeField] private float marginBehind = 40f;
        [SerializeField] private float marginAhead = 120f;

        public void Generate(LevelConfig levelConfig)
        {
            float endZ = levelConfig.levelLength + marginAhead;
            for (float z = -marginBehind; z < endZ; z += tileLength)
            {
                Instantiate(groundPrefab, new Vector3(0f, 0f, z + tileLength * 0.5f), Quaternion.identity, root);
            }

            Instantiate(finishLinePrefab, new Vector3(0f, 0f, levelConfig.levelLength), Quaternion.identity, root);
        }
    }
}
