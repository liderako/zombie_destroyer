using HcCoreConfigs.Settings;
using UnityEngine;

namespace Entities.Zombies
{
    // Moves the body for the GOAP actions: the chase (AttackCarAction) and the walk to the wander point (WanderAction)
    public class ZombieMovementComponent : MonoBehaviour
    {
        private const float APPROACH_FADE_DISTANCE = 8f;
        private const float MIN_WOBBLE_FREQUENCY = 0.6f;
        private const float MAX_WOBBLE_FREQUENCY = 1.2f;

        [SerializeField] private ZombieComponent zombie;
        [SerializeField] private ZombieViewComponent view;

        private float approachOffset;
        private float wobblePhase;
        private float wobbleFrequency;

        // Per-zombie approach offset and wobble, so the crowd doesn't run along the same lines
        private void Awake()
        {
            ZombieSettings settings = zombie.Settings;
            approachOffset = Random.Range(-settings.approachSpread, settings.approachSpread);
            wobblePhase = Random.Range(0f, 2f * Mathf.PI);
            wobbleFrequency = Random.Range(MIN_WOBBLE_FREQUENCY, MAX_WOBBLE_FREQUENCY) * 2f * Mathf.PI;
        }

        private void Update()
        {
            // Paused: the speed would be NaN and stick in the damped animator parameter
            if (Time.deltaTime <= 0f)
            {
                return;
            }

            ZombieSettings settings = zombie.Settings;
            float speed = 0f;
            if (zombie.IsChasing)
            {
                Vector3 destination = GetDestination(settings);
                // Faces the run direction, turns to the car only to hit it
                Vector3 lookDirection = zombie.IsCarInReach
                    ? zombie.Car.transform.position - transform.position
                    : destination - transform.position;
                speed = MoveTo(destination, zombie.RunSpeed, lookDirection, settings.turnSpeed);
            }
            else if (zombie.IsWalkingToTarget)
            {
                Vector3 destination = zombie.TargetPosition;
                speed = MoveTo(destination, settings.walkSpeed, destination - transform.position, settings.walkTurnSpeed);
            }

            view.SetMoveSpeed(speed);
        }

        // Returns the actual speed, m/s
        private float MoveTo(Vector3 destination, float maxSpeed, Vector3 lookDirection, float turnSpeed)
        {
            Vector3 position = transform.position;
            destination.y = position.y;
            Vector3 nextPosition = Vector3.MoveTowards(position, destination, maxSpeed * Time.deltaTime);
            transform.position = nextPosition;

            lookDirection.y = 0f;
            if (lookDirection.sqrMagnitude > 0.001f)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(lookDirection),
                    turnSpeed * Time.deltaTime);
            }

            return (nextPosition - position).magnitude / Time.deltaTime;
        }

        private Vector3 GetDestination(ZombieSettings settings)
        {
            Vector3 position = transform.position;
            Vector3 closestPoint = zombie.Car.ClosestPointOnBody(position);
            Vector3 fromBody = position - closestPoint;
            fromBody.y = 0f;

            if (fromBody.magnitude <= settings.closeDistance)
            {
                return GetAttackPoint(settings, position, closestPoint, fromBody);
            }

            // Run to the intercept point, so zombies come from the front and sides
            Vector3 carPosition = zombie.Car.transform.position;
            Vector3 carVelocity = zombie.Car.Movement.Velocity;
            float leadTime = Mathf.Min(GetInterceptTime(carPosition - position, carVelocity, zombie.RunSpeed), settings.maxLeadTime);
            Vector3 intercept = carPosition + carVelocity * leadTime;

            // Offset and wobble fade out near the car
            float farness = Mathf.Clamp01((fromBody.magnitude - settings.closeDistance) / APPROACH_FADE_DISTANCE);
            Vector3 toIntercept = intercept - position;
            toIntercept.y = 0f;
            Vector3 sideways = Vector3.Cross(Vector3.up, toIntercept.normalized);
            float wobble = Mathf.Sin(Time.time * wobbleFrequency + wobblePhase) * settings.runWobble;
            return intercept + (zombie.Car.transform.right * approachOffset + sideways * wobble) * farness;
        }

        // Earliest t > 0 where |toCar + carVelocity * t| = runSpeed * t; 0 if there is none (runs straight at the car)
        private static float GetInterceptTime(Vector3 toCar, Vector3 carVelocity, float runSpeed)
        {
            toCar.y = 0f;
            carVelocity.y = 0f;
            float a = carVelocity.sqrMagnitude - runSpeed * runSpeed;
            float b = 2f * Vector3.Dot(toCar, carVelocity);
            float c = toCar.sqrMagnitude;

            if (Mathf.Abs(a) < 0.0001f)
            {
                return b < 0f ? -c / b : 0f;
            }

            float discriminant = b * b - 4f * a * c;
            if (discriminant < 0f)
            {
                return 0f;
            }

            float root = Mathf.Sqrt(discriminant);
            float first = (-b - root) / (2f * a);
            float second = (-b + root) / (2f * a);
            float earliest = Mathf.Min(first, second);
            float latest = Mathf.Max(first, second);
            if (earliest > 0f)
            {
                return earliest;
            }

            return latest > 0f ? latest : 0f;
        }

        // Attack point at the front or a side of the car: a zombie behind it moves up to the side,
        // so the turret never has to shoot towards the camera
        private Vector3 GetAttackPoint(ZombieSettings settings, Vector3 position, Vector3 closestPoint, Vector3 fromBody)
        {
            Transform car = zombie.Car.transform;
            float standOff = settings.attackReach * 0.6f;
            Vector3 local = car.InverseTransformPoint(position);

            if (local.z < settings.rearAttackLimit)
            {
                Bounds body = zombie.Car.BodyBounds;
                float side = local.x >= body.center.x ? 1f : -1f;
                var sidePoint = new Vector3(body.center.x + side * (body.extents.x + standOff), 0f, settings.rearAttackLimit);
                return car.TransformPoint(sidePoint);
            }

            Vector3 outward = fromBody.sqrMagnitude > 0.0001f ? fromBody.normalized : car.forward;
            return closestPoint + outward * standOff;
        }
    }
}
