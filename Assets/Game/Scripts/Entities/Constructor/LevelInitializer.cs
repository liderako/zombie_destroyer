using CoreGameplay.Components;
using CoreGameplay.Components.Interfaces;
using Game.Scripts.CoreGameplay.Components;
using PencilFlow.Configs;
using UnityEngine;
using VContainer;

namespace Entities.Constructor
{
    public class LevelInitializer : MonoBehaviour
    {
        [Inject] private LevelManager levelManager;
        [Inject] private ProgressTrackerComponent progressTracker;
        [Inject] private IAnalyticsComponent analytics;

        [SerializeField] private GroundInitializer groundInitializer;
        [SerializeField] private ZombiesInitializer zombiesInitializer;

        private void Start()
        {
            var levelConfig = (LevelConfig)levelManager.GetCurrentLevelItem();

            groundInitializer.Generate(levelConfig);
            zombiesInitializer.Generate(levelConfig);
            progressTracker.SetupDefaultProgress(levelConfig.levelLength);

            analytics.SendLevelStartedEvent(levelManager.GetCurrentLevelForUI());
        }
    }
}
