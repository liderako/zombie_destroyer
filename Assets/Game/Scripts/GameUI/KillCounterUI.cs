using DG.Tweening;
using Entities.Blackboards;
using GeneralUI.Components;
using UnityEngine;
using VContainer;

namespace GameUI
{
    public class KillCounterUI : MonoBehaviour
    {
        [Inject] private ZombiesBlackboard blackboard;

        [SerializeField] private CounterUI counter;
        [SerializeField] private Vector3 killPunch = new Vector3(0.2f, 0.2f, 0f);
        [SerializeField] private float punchDuration = 0.2f;

        private void Start()
        {
            counter.Set(blackboard.KilledCount);
        }

        private void OnEnable()
        {
            blackboard.OnZombieKilled += OnHandleZombieKilled;
        }

        private void OnDisable()
        {
            blackboard.OnZombieKilled -= OnHandleZombieKilled;
        }

        private void OnHandleZombieKilled(int killedCount)
        {
            counter.Set(killedCount);

            counter.Icon.DOKill(true);
            counter.Icon.DOPunchScale(killPunch, punchDuration);
        }
    }
}
