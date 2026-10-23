using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Scripts.CoreGameplay.Components.Config
{
    [CreateAssetMenu(fileName = "LevelItem", menuName = "ScriptableObjects/LevelItem", order = 0)]
    public class LevelItem : SerializedScriptableObject
    {
        public Vector2Int rewardMoneyRange = new Vector2Int(180, 220);

        public int priceRevive = 100;

        public int GetRewardForLevel()
        {
            return Random.Range(rewardMoneyRange.x, rewardMoneyRange.y);
        }
    }
}