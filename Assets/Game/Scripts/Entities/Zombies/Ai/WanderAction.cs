using CrashKonijn.Agent.Core;
using CrashKonijn.Agent.Runtime;
using CrashKonijn.Goap.Runtime;

namespace Entities.Zombies.Ai
{
    // Idle wandering: walks to the wander point, waits there, then picks the next one.
    // The Aggroed effect only lets the planner chain it before AttackCarAction; the sensor sets the real value
    public class WanderAction : GoapActionBase<WanderAction.Data>
    {
        public override void Start(IMonoAgent agent, Data data)
        {
            data.IdleTimeLeft = data.Zombie.NextWanderPause();
        }

        public override IActionRunState Perform(IMonoAgent agent, Data data, IActionContext context)
        {
            data.IdleTimeLeft -= context.DeltaTime;
            if (data.IdleTimeLeft > 0f)
            {
                return ActionRunState.ContinueOrResolve;
            }

            data.Zombie.PickWanderPoint();
            return ActionRunState.Completed;
        }

        public class Data : IActionData
        {
            public ITarget Target { get; set; }

            [GetComponent]
            public ZombieComponent Zombie { get; set; }

            public float IdleTimeLeft { get; set; }
        }
    }
}
