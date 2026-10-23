using Dythervin.UpdateSystem.Main;

namespace Dythervin.UpdateSystem
{
    public static partial class UpdatablesExt
    {
        public static void EnableUpdater(this IUpdatable updatable)
        {
            Updater.Instance.Set(updatable, true);
        }

        public static void EnableUpdater(this IUpdatableDelta updatable)
        {
            Updater.Instance.Set(updatable, true);
        }

        public static void EnableUpdatableInterval(this IUpdatableInterval updatable)
        {
            UpdaterInterval.Instance.Set(updatable, true);
        }

        public static void EnableFixedUpdater(this IUpdatableFixed updatable)
        {
            UpdaterFixed.Instance.Set(updatable, true);
        }

        public static void EnableFixedUpdater(this IUpdatableFixedDelta updatable)
        {
            UpdaterFixed.Instance.Set(updatable, true);
        }

        public static void EnableLateUpdater(this IUpdatableLate updatable)
        {
            UpdaterLate.Instance.Set(updatable, true);
        }

        public static void EnableLateUpdater(this IUpdatableLateDelta updatable)
        {
            UpdaterLate.Instance.Set(updatable, true);
        }

        public static void DisableUpdater(this IUpdatable updatable)
        {
            Updater.Instance.Set(updatable, false);
        }

        public static void DisableUpdater(this IUpdatableDelta updatable)
        {
            Updater.Instance.Set(updatable, false);
        }

        public static void DisableUpdatableInterval(this IUpdatableInterval updatable)
        {
            UpdaterInterval.Instance.Set(updatable, false);
        }

        public static void DisableFixedUpdater(this IUpdatableFixed updatable)
        {
            UpdaterFixed.Instance.Set(updatable, false);
        }

        public static void DisableFixedUpdater(this IUpdatableFixedDelta updatable)
        {
            UpdaterFixed.Instance.Set(updatable, false);
        }

        public static void DisableLateUpdater(this IUpdatableLate updatable)
        {
            UpdaterLate.Instance.Set(updatable, false);
        }

        public static void DisableLateUpdater(this IUpdatableLateDelta updatable)
        {
            UpdaterLate.Instance.Set(updatable, false);
        }
    }
}