using Core.VContainer;
using UnityEngine;
using VContainer;

namespace Entities.Blackboards
{
    public class ZombiesBlackboardInstaller : BaseInstaller
    {
        [SerializeField] private ZombiesBlackboard source;

        public override void Install(IContainerBuilder builder)
        {
            builder.RegisterInstance(source).AsSelf();
        }
    }
}
