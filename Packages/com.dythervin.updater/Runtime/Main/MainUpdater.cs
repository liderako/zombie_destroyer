using Dythervin.Core.Utils;
using UnityEngine;

namespace Dythervin.UpdateSystem.Main
{
    public static class MainUpdater
    {
        private static MainUpdaterInternal _mainUpdaterInternal;

        public static bool AutoUpdaterEnabled
        {
            get => _mainUpdaterInternal.enabled;
            set => _mainUpdaterInternal.enabled = value;
        }

        ///Shorthand for Updater.Instance.Update(deltaTime);
        public static void Update(float deltaTime)
        {
            Updater.Instance.Update(deltaTime);
            UpdaterInterval.Instance.Update(deltaTime);
        }

        ///Shorthand for UpdaterFixed.Instance.Update(deltaTime);
        public static void FixedUpdate(float deltaTime)
        {
            UpdaterFixed.Instance.Update(deltaTime);
        }

        ///Shorthand for UpdaterLate.Instance.Update(deltaTime);
        public static void LateUpdate(float deltaTime)
        {
            UpdaterLate.Instance.Update(deltaTime);
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void InitStatic()
        {
            GameObject gameObject = PersistentRoot.Get("MainUpdater");
            _mainUpdaterInternal = gameObject.AddComponent<MainUpdaterInternal>();
            gameObject.hideFlags = HideFlags.NotEditable;
        }
    }
}