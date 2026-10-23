using System;
using UnityEngine;

namespace Gameplay.Stack.Item.Utils
{
    public class ItemAnim : MonoBehaviour, IItemAnimationEventDisappear
    {
        private static readonly int BounceHash = Animator.StringToHash("Bounce");
        private static readonly int DisappearHash = Animator.StringToHash("Disappear");

        public enum Anim : byte
        {
            None = 0,
            Bounce = 1,
            Disappear = 2
        }


        [SerializeField] private ItemMoverWorldBase moverWorld;
        [SerializeField] private Animator animator;

        public event Action OnDisappeared;

        public Anim reachedAnim = Anim.Bounce;

        private void Awake()
        {
            moverWorld.OnReached += OnReached;
        }

        private void OnDisable()
        {
            transform.localScale = Vector3.one;
            reachedAnim = Anim.Bounce;
        }

        private void OnReached()
        {
            if (reachedAnim == Anim.None)
                return;

            int hash = reachedAnim switch
            {
                Anim.Bounce => BounceHash,
                Anim.Disappear => DisappearHash,
                _ => throw new ArgumentOutOfRangeException()
            };
            animator.SetTrigger(hash);
        }

        private void Reset()
        {
            TryGetComponent(out moverWorld);
            TryGetComponent(out animator);
        }

        public void Disappeared()
        {
            OnDisappeared?.Invoke();
            gameObject.SetActive(false);
        }
    }
}