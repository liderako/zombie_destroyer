using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Core.VContainer
{
    public abstract class BaseInstaller : MonoBehaviour, IInstaller
    {
        public abstract void Install(IContainerBuilder builder);
    }
}