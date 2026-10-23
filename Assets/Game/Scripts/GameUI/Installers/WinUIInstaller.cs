using Core.VContainer;
using GameUI;
using UnityEngine;
using VContainer;

namespace GameUI.Installers
{
    public class WinUIInstaller : BaseInstaller
    {
        [SerializeField] private WinUI source;

        public override void Install(IContainerBuilder builder)
        {
            builder.RegisterInstance(source).AsSelf();
        }
    }
}
