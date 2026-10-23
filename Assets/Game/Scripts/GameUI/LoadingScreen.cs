using CoreGameplay.Components;
using UnityEngine;
using VContainer;

namespace GameUI
{
    public class LoadingScreen : MonoBehaviour
    {
        [Inject] private LevelManager levelManager;

        private void Start()
        {
            levelManager.LoadLevelByName("GameplayScene");
        }
    }
}