using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Entities.Zombies
{
    // AC_Zombie: idle / walk / run blended by speed, the attack on the upper-body layer, the death
    public class ZombieViewComponent : MonoBehaviour
    {
        private const string ATTACK_LAYER = "Attack";
        private const string LOCOMOTION_STATE = "Locomotion";

        private static readonly int SpeedHash = Animator.StringToHash("Speed");
        private static readonly int RunPlaybackHash = Animator.StringToHash("RunPlayback");
        private static readonly int AttackHash = Animator.StringToHash("Attack");
        private static readonly int DieHash = Animator.StringToHash("Die");

        [SerializeField] private Animator animator;
        [SerializeField] private Transform view;
        [SerializeField] private ParticleSystem hitVfx;

        [Header("Locomotion")]
        [Tooltip("Speed of the run clip in m/s: a faster zombie plays it faster")]
        [SerializeField] private float runClipSpeed = 3.2f;
        [Tooltip("Upper limit of the run playback rate, so a fast zombie doesn't flail")]
        [SerializeField] private float maxRunPlayback = 2.2f;
        [SerializeField] private float speedDampTime = 0.1f;

        [Header("Hit")]
        [SerializeField] private Vector3 hitPunch = new Vector3(0.2f, -0.15f, 0.2f);
        [SerializeField] private float hitPunchDuration = 0.15f;

        [Header("Disappear")]
        [SerializeField] private float disappearDuration = 0.3f;

        private int attackLayer;

        private void Awake()
        {
            attackLayer = animator.GetLayerIndex(ATTACK_LAYER);
            // Random cycle phase, so zombies don't step in sync
            animator.Play(LOCOMOTION_STATE, 0, Random.value);
        }

        /// <param name="speed">Movement speed in m/s</param>
        public void SetMoveSpeed(float speed)
        {
            animator.SetFloat(SpeedHash, speed, speedDampTime, Time.deltaTime);
            animator.SetFloat(RunPlaybackHash, Mathf.Clamp(speed / runClipSpeed, 1f, maxRunPlayback));
        }

        public void PlayAttack()
        {
            animator.SetTrigger(AttackHash);
        }

        public void PlayHit()
        {
            hitVfx.Play();
            view.DOKill(true);
            view.DOPunchScale(hitPunch, hitPunchDuration).SetLink(gameObject);
        }

        public void PlayDeath()
        {
            // The attack layer must not play over the death clip
            animator.SetLayerWeight(attackLayer, 0f);
            animator.SetTrigger(DieHash);
            PlayHit();
        }

        public async UniTask PlayDisappearAsync()
        {
            view.DOKill(true);
            await view.DOScale(0f, disappearDuration).SetEase(Ease.InBack).SetLink(gameObject).AsyncWaitForCompletion();
        }
    }
}
