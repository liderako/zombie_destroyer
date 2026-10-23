using Core.VContainer;
using Game.Scripts.CoreGameplay.Components;
using UnityEngine;
using VContainer;

namespace CoreGameplay.Components.Installers
{
    public class ProgressTrackerInstaller : BaseInstaller
    {
        [SerializeField] private ProgressTrackerComponent source;

        public override void Install(IContainerBuilder builder)
        {
            builder.RegisterInstance(source).AsSelf();
        }
    }
}
