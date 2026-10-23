using CrashKonijn.Agent.Core;
using CrashKonijn.Goap.Core;
using CrashKonijn.Goap.Runtime;
using HcCoreConfigs.Settings;
using UnityEngine;

namespace Entities.Zombies.Ai
{
    public class ZombieAgentTypeFactory : AgentTypeFactoryBase
    {
        public const string Id = "Zombie";
        private const float WANDER_STOPPING_DISTANCE = 0.15f;

        [SerializeField] private ZombieSettings settings;

        public override IAgentTypeConfig Create()
        {
            AgentTypeBuilder builder = CreateBuilder(Id);

            builder.CreateCapability("AttackCar", capability =>
            {
                capability.AddGoal<AttackCarGoal>()
                    .AddCondition<CarAttacked>(Comparison.GreaterThanOrEqual, 1);

                capability.AddAction<WanderAction>()
                    .AddEffect<Aggroed>(EffectType.Increase)
                    .SetTarget<WanderTarget>()
                    .SetMoveMode(ActionMoveMode.MoveBeforePerforming)
                    .SetStoppingDistance(WANDER_STOPPING_DISTANCE);

                capability.AddAction<AttackCarAction>()
                    .AddCondition<Aggroed>(Comparison.GreaterThanOrEqual, 1)
                    .AddEffect<CarAttacked>(EffectType.Increase)
                    .SetTarget<CarTarget>()
                    .SetMoveMode(ActionMoveMode.PerformWhileMoving)
                    .SetStoppingDistance(settings.attackReach);

                capability.AddMultiSensor<ZombieSensors>();
            });

            return builder.Build();
        }
    }
}
