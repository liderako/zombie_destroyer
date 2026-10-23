using Dythervin.Core;
using UnityEngine;

namespace Dythervin.UpdateSystem.Main
{
    [AddComponentMenu("")]
    internal class MainUpdaterInternal : SingletonMono<IMainUpdaterInternal, MainUpdaterInternal>, IMainUpdaterInternal
    {
        private void Update()
        {
            float deltaTime = Time.deltaTime;
            Updater.Instance.Update(deltaTime);
            UpdaterInterval.Instance.Update(deltaTime);
        }

        private void FixedUpdate()
        {
            UpdaterFixed.Instance.Update(Time.fixedDeltaTime);
        }

        private void LateUpdate()
        {
            UpdaterLate.Instance.Update(Time.deltaTime);
        }
    }

    internal interface IMainUpdaterInternal
    {
    }
}