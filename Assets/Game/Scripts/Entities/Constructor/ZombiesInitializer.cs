using System.Collections.Generic;
using Entities.Blackboards;
using Entities.Zombies;
using PencilFlow.Configs;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Entities.Constructor
{
    // Created once per level: a restart reloads the scene, so a pool would never reuse them
    public class ZombiesInitializer : MonoBehaviour
    {
        [Inject] private IObjectResolver resolver;
        [Inject] private ZombiesBlackboard blackboard;

        [SerializeField] private ZombieComponent zombiePrefab;
        [SerializeField] private Transform root;
        [SerializeField] private int placementAttempts = 30;

        public void Generate(LevelConfig levelConfig)
        {
            foreach (Vector3 position in GeneratePositions(levelConfig))
            {
                Quaternion rotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
                ZombieComponent zombie = resolver.Instantiate(zombiePrefab, position, rotation, root);
                blackboard.AddZombie(zombie);
            }
        }

        private List<Vector3> GeneratePositions(LevelConfig levelConfig)
        {
            var positions = new List<Vector3>(levelConfig.zombiesCount);
            float maxZ = levelConfig.levelLength - levelConfig.spawnEndOffset;
            float minSpacingSqr = levelConfig.minZombieSpacing * levelConfig.minZombieSpacing;

            for (int i = 0; i < levelConfig.zombiesCount; i++)
            {
                for (int attempt = 0; attempt < placementAttempts; attempt++)
                {
                    var candidate = new Vector3(
                        Random.Range(-levelConfig.spawnHalfWidth, levelConfig.spawnHalfWidth),
                        0f,
                        Random.Range(levelConfig.spawnStartZ, maxZ));

                    if (IsFarEnough(candidate, positions, minSpacingSqr))
                    {
                        positions.Add(candidate);
                        break;
                    }
                }
            }

            return positions;
        }

        private static bool IsFarEnough(Vector3 candidate, List<Vector3> positions, float minSpacingSqr)
        {
            foreach (Vector3 position in positions)
            {
                if ((position - candidate).sqrMagnitude < minSpacingSqr)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
