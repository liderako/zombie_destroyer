using Core.VContainer;
using UnityEngine;
using VContainer;

namespace Game.Packages.Joystick_Pack.Installers
{
    public class VariableJoystickInstaller : BaseInstaller
    {
        [SerializeField] private VariableJoystick source;
        
        public override void Install(IContainerBuilder builder)
        {
            builder.RegisterInstance(source).AsSelf();
        }
    }
}