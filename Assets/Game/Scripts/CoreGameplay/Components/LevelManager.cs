using System;
using System.Threading.Tasks;
using Components.CoreGameplay;
using Game.Scripts.CoreGameplay.Components.Config;
using Memento;
using Memento.Interfaces;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CoreGameplay.Components
{
    public class LevelManager : MonoBehaviour, IMementoComponent
    {
        [SerializeField] private LevelItem[] levelItems;
        private int currentLevel;
        private int currentIndexLevel;
        
        public Action OnReleasePoolsOnScene;

        private void Awake()
        {
            Load();
        }

        private void Start()
        {
#if DEV_CONTEXT
            if (!SRDebug.IsInitialized)
            {
                SRDebug.Init();
                SROptions.Current.LevelManager = this;
            }
            else
            {
                SROptions.Current.LevelManager = this;
            }
#endif
        }

        public void LoadLevelByName(string sceneName)
        {
            _ = LoadSceneAndUnloadUnused(sceneName);
        }

        public void LoadCurrentLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void LoadNextLevel()
        {
            FinishLevel();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        
        public async Task LoadSceneAndUnloadUnused(string sceneName)
        {
            var op = SceneManager.LoadSceneAsync(sceneName);

            while (!op.isDone)
                await Task.Yield();
            _ = Resources.UnloadUnusedAssets();
        }
        
        public int GetCurrentLevelForUI()
        {
            return currentLevel + 1;
        }
        
        public int GetCurrentIndexLevel()
        {
            return currentIndexLevel;
        }

        public void FinishLevel()
        {
            currentLevel++;
            UpdateIndexLevel();
            Save();
            MementoSystem.StoreCacheFile();
        }
        
        public void SetLevel(int level)
        {
            currentLevel = level;
            UpdateIndexLevel();
        }

        private void UpdateIndexLevel()
        {
            currentIndexLevel = currentLevel % levelItems.Length;
        }

        public int GetNextIndexLevel()
        {
            return (currentLevel + 1) % levelItems.Length;
        }

        public void Save()
        {
            MementoSystem.Save(KeyTerms.LvlKey, currentLevel);
        }

        public void Load()
        {
            SetLevel(MementoSystem.Load(KeyTerms.LvlKey, 0));
        }

        public LevelItem GetCurrentLevelItem()
        {
            return levelItems[currentIndexLevel];
        }

        public void CallReleasePools()
        {
            OnReleasePoolsOnScene?.Invoke();
        }
    }
}