using Core.VContainer;
using UnityEngine;
using VContainer;

namespace Components.CoreGameplay.Behavior.Installers
{
    public class SpriteFlyBehaviorComponentInstaller : BaseInstaller
    {
        [SerializeField] private SpriteFlyBehaviorComponent source;
        
        public override void Install(IContainerBuilder builder)
        {
            builder.RegisterInstance(source).AsSelf();
        }
    }
}