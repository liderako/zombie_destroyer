using Core.VContainer;
using UnityEngine;
using VContainer;

namespace TurnBaseGame.Scripts.Core.Installers
{
    public class MainCameraInstaller : BaseInstaller
    {
        [SerializeField] private Camera camera;
        
        public override void Install(IContainerBuilder builder)
        {
            builder.RegisterInstance(camera).AsSelf();
        }
    }
}