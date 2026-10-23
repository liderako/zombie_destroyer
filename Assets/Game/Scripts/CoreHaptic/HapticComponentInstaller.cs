using Core.VContainer;
using UnityEngine;
using VContainer;

namespace Haptic.Components
{
    public class HapticComponentInstaller : BaseInstaller
    {
        [SerializeField] private HapticComponent source;
        
        public override void Install(IContainerBuilder builder)
        {
            builder.RegisterInstance(source).AsImplementedInterfaces();
        }
    }
}