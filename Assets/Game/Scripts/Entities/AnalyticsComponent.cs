using CoreGameplay.Components.Interfaces;
using UnityEngine;

namespace Entities
{
    // Stub until an analytics SDK is connected
    public class AnalyticsComponent : MonoBehaviour, IAnalyticsComponent
    {
        public void SendLevelStartedEvent(int levelNumber)
        {
            Debug.Log($"[Analytics] Level {levelNumber} started");
        }

        public void SendLevelCompletedEvent(bool win)
        {
            Debug.Log($"[Analytics] Level completed, win: {win}");
        }
    }
}
