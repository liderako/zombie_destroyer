using CrashKonijn.Goap.Runtime;
using UnityEngine;

namespace Entities.Zombies.Ai
{
    public class ZombieSensors : MultiSensorBase
    {
        private const float AGGRO_SENSE_INTERVAL = 0.2f;

        public ZombieSensors()
        {
            AddLocalWorldSensor<Aggroed>(
                (agent, references) => references.GetCachedComponent<ZombieComponent>().IsAggroed,
                SensorTimer.Interval(AGGRO_SENSE_INTERVAL));

            AddLocalTargetSensor<CarTarget>(
                (agent, references, target) => target ?? new TransformTarget(references.GetCachedComponent<ZombieComponent>().Car.transform));

            // Returns the stored point: sensors run on every resolve, a random point here would change every 0.25 s
            AddLocalTargetSensor<WanderTarget>((agent, references, target) =>
            {
                Vector3 point = references.GetCachedComponent<ZombieComponent>().WanderPoint;
                return target is PositionTarget positionTarget ? positionTarget.SetPosition(point) : new PositionTarget(point);
            });
        }

        public override void Created()
        {
        }

        public override void Update()
        {
        }
    }
}
