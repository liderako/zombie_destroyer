using Core.VContainer;
using GameUI;
using UnityEngine;
using VContainer;

namespace GameUI.Installers
{
    public class SettingsUIInstaller : BaseInstaller
    {
        [SerializeField] private SettingsUI source;

        public override void Install(IContainerBuilder builder)
        {
            builder.RegisterInstance(source).AsSelf();
        }
    }
}
