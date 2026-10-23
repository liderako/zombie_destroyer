using CrashKonijn.Goap.Runtime;

namespace Entities.Zombies.Ai
{
    // The car is in the aggro range or a bullet hit the zombie
    public class Aggroed : WorldKeyBase
    {
    }

    // Never sensed, so AttackCarGoal is never complete and the zombie keeps attacking
    public class CarAttacked : WorldKeyBase
    {
    }

    public class CarTarget : TargetKeyBase
    {
    }

    // Next point of the idle walk near the spawn point
    public class WanderTarget : TargetKeyBase
    {
    }
}
