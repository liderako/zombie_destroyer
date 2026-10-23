using System;
using UnityEngine;

namespace Game.Scripts.CoreGameplay.Components
{
    public class ProgressTrackerComponent : MonoBehaviour
    {
        public int MaxProgressValue { get; private set; }
        public int CurrentProgressValue { get; private set; }
        
        public Action OnStartGame;
        public Action<int, int> OnUpdateProgress;
        public Action OnFinish;
        public Action OnFailGame;
        public Action OnPreFailGame;

        public void SetupDefaultProgress(int maxAmount)
        {
            MaxProgressValue = maxAmount;
            CurrentProgressValue = 0;
        }

        public void CallStartGameEvent()
        {
            OnStartGame?.Invoke();
        }

        public void UpdateProgress(int amount)
        {
            CurrentProgressValue += amount;
            OnUpdateProgress?.Invoke(CurrentProgressValue, MaxProgressValue);
            if (CurrentProgressValue >= MaxProgressValue)
            {
                OnFinish?.Invoke();
            }
        }

        public void CallPreFailGameEvent()
        {
            OnPreFailGame?.Invoke();
        }

        public void CallFailGameEvent()
        {
            OnFailGame?.Invoke();
        }
    }
}