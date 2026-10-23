using System;
using System.Threading;
using CrashKonijn.Agent.Core;
using CrashKonijn.Agent.Runtime;
using CrashKonijn.Goap.Runtime;
using Cysharp.Threading.Tasks;
using Entities.Blackboards;
using Entities.Car;
using Entities.Combat;
using Entities.Zombies.Ai;
using HcCoreConfigs.Settings;
using UnityEngine;
using VContainer;

namespace Entities.Zombies
{
    public class ZombieComponent : MonoBehaviour, IDamageable
    {
        [SerializeField] private ZombieSettings settings;
        [SerializeField] private AgentBehaviour agent;
        [SerializeField] private GoapActionProvider actionProvider;
        [SerializeField] private ZombieViewComponent view;
        [SerializeField] private Rigidbody body;

        private ZombiesBlackboard blackboard;
        private bool isProvoked;
        private Vector3 homePosition;
        private float wanderSide;
        private bool isFirstWanderPause = true;

        public CarComponent Car { get; private set; }
        public ZombieSettings Settings => settings;
        public Health Health { get; private set; }
        public bool IsChasing { get; private set; }
        public Vector3 WanderPoint { get; private set; }
        public float RunSpeed { get; private set; }

        // WanderAction moves the agent to its target before performing
        public bool IsWalkingToTarget => !agent.IsPaused && agent.State == AgentState.MovingToTarget && agent.CurrentTarget != null;
        public Vector3 TargetPosition => agent.CurrentTarget.Position;

        // The car is in the aggro range, or a bullet hit the zombie
        public bool IsAggroed => !Health.IsDead && (isProvoked || IsCarInAggroRange);

        private bool IsCarInAggroRange =>
            (Car.transform.position - transform.position).sqrMagnitude <= settings.aggroRadius * settings.aggroRadius;

        public bool IsCarInReach => DistanceToCarBody() <= settings.attackReach;

        [Inject]
        public void Construct(CarComponent car, ZombiesBlackboard blackboard, GoapBehaviour goap)
        {
            Car = car;
            this.blackboard = blackboard;
            // Set before the provider's Start: it throws without an agent type
            actionProvider.AgentType = goap.GetAgentType(ZombieAgentTypeFactory.Id);
        }

        private void Awake()
        {
            Health = new Health(settings.maxHealth);
            RunSpeed = settings.runSpeed * UnityEngine.Random.Range(1f - settings.runSpeedSpread, 1f + settings.runSpeedSpread);
            Health.Damaged += OnHandleDamaged;
        }

        private void Start()
        {
            homePosition = transform.position;
            // Half of the zombies start walking right away, the rest wait first
            wanderSide = UnityEngine.Random.value < 0.5f ? -1f : 1f;
            WanderPoint = homePosition;
            if (UnityEngine.Random.value < 0.5f)
            {
                PickWanderPoint();
            }
            actionProvider.RequestGoal<AttackCarGoal>();
        }

        // The first pause can be zero, so zombies start moving at different times
        public float NextWanderPause()
        {
            Vector2 range = settings.wanderIdleTime;
            float pause = UnityEngine.Random.Range(isFirstWanderPause ? 0f : range.x, range.y);
            isFirstWanderPause = false;
            return pause;
        }

        // Next point on the other side of the spawn point: the zombie paces left and right
        public void PickWanderPoint()
        {
            wanderSide = -wanderSide;
            float sideways = wanderSide * UnityEngine.Random.Range(0.4f, 1f) * settings.wanderRadius;
            float along = UnityEngine.Random.Range(-0.3f, 0.3f) * settings.wanderRadius;
            WanderPoint = homePosition + new Vector3(sideways, 0f, along);
        }

        public void TakeDamage(float damage, Vector3 direction)
        {
            if (Health.IsDead)
            {
                return;
            }

            Health.TakeDamage(damage);

            if (Health.IsDead)
            {
                Die(direction);
            }
            else
            {
                isProvoked = true;
                view.PlayHit();
            }
        }

        public void AttackCar()
        {
            Vector3 direction = (Car.transform.position - transform.position).normalized;
            Car.TakeDamage(settings.attackDamage, direction);
            view.PlayAttack();
        }

        public void BeginChase()
        {
            IsChasing = true;
        }

        public void EndChase()
        {
            IsChasing = false;
        }

        public void Freeze()
        {
            IsChasing = false;
            agent.IsPaused = true;
        }

        public float DistanceToCarBody()
        {
            Vector3 offset = Car.ClosestPointOnBody(transform.position) - transform.position;
            offset.y = 0f;
            return offset.magnitude;
        }

        private void OnHandleDamaged(float damage)
        {
            blackboard.NotifyDamaged(this, damage);
        }

        private void Die(Vector3 hitDirection)
        {
            Freeze();
            gameObject.layer = LayerMask.NameToLayer(settings.corpseLayer);

            // The death clip lays the body down; physics only throws it back along the bullet
            body.isKinematic = false;
            body.constraints = RigidbodyConstraints.FreezeRotation;
            body.AddForce((hitDirection + Vector3.up * 0.5f).normalized * settings.deathImpulse, ForceMode.VelocityChange);

            view.PlayDeath();
            blackboard.NotifyKilled(this);
            HideCorpseAsync(destroyCancellationToken).Forget();
        }

        private async UniTaskVoid HideCorpseAsync(CancellationToken token)
        {
            bool isCanceled = await UniTask.Delay(TimeSpan.FromSeconds(settings.corpseLifetime), cancellationToken: token)
                .SuppressCancellationThrow();
            if (isCanceled)
            {
                return;
            }

            await view.PlayDisappearAsync();
            // Destroyed by a scene reload: the linked tween was killed and the await returned
            if (token.IsCancellationRequested)
            {
                return;
            }

            gameObject.SetActive(false);
        }
    }
}
