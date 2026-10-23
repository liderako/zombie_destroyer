using CrashKonijn.Agent.Core;
using CrashKonijn.Agent.Runtime;
using CrashKonijn.Goap.Runtime;

namespace Entities.Zombies.Ai
{
    // Runs to the car and hits it while in reach. Performs while moving, because the car keeps driving away
    public class AttackCarAction : GoapActionBase<AttackCarAction.Data>
    {
        public override void Start(IMonoAgent agent, Data data)
        {
            data.Zombie.BeginChase();
            data.Cooldown = data.Zombie.Settings.attackInterval * 0.5f;
        }

        public override IActionRunState Perform(IMonoAgent agent, Data data, IActionContext context)
        {
            if (!data.Zombie.IsCarInReach)
            {
                return ActionRunState.Continue;
            }

            data.Cooldown -= context.DeltaTime;
            if (data.Cooldown <= 0f)
            {
                data.Cooldown = data.Zombie.Settings.attackInterval;
                data.Zombie.AttackCar();
            }

            return ActionRunState.Continue;
        }

        public override void End(IMonoAgent agent, Data data)
        {
            data.Zombie.EndChase();
        }

        public class Data : IActionData
        {
            public ITarget Target { get; set; }

            [GetComponent]
            public ZombieComponent Zombie { get; set; }

            public float Cooldown { get; set; }
        }
    }
}
