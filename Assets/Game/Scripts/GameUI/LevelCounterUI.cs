using CoreGameplay.Components;
using TMPro;
using UnityEngine;
using VContainer;

namespace GameUI
{
    public class LevelCounterUI : MonoBehaviour
    {
        [Inject] private LevelManager levelManager;

        [SerializeField] private TMP_Text label;

        private void Start()
        {
            label.SetText("Level {0}", levelManager.GetCurrentLevelForUI());
        }
    }
}
