using Core.VContainer;
using GameUI;
using UnityEngine;
using VContainer;

namespace GameUI.Installers
{
    public class TimeReviveUIInstaller : BaseInstaller
    {
        [SerializeField] private TimeReviveUI source;

        public override void Install(IContainerBuilder builder)
        {
            builder.RegisterInstance(source).AsSelf();
        }
    }
}
