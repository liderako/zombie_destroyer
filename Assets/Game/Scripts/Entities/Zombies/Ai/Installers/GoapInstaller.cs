using Core.VContainer;
using CrashKonijn.Goap.Runtime;
using UnityEngine;
using VContainer;

namespace Entities.Zombies.Ai.Installers
{
    public class GoapInstaller : BaseInstaller
    {
        [SerializeField] private GoapBehaviour source;

        public override void Install(IContainerBuilder builder)
        {
            builder.RegisterInstance(source).AsSelf();
        }
    }
}
