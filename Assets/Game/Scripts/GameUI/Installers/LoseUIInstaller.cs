using Core.VContainer;
using GameUI;
using UnityEngine;
using VContainer;

namespace GameUI.Installers
{
    public class LoseUIInstaller : BaseInstaller
    {
        [SerializeField] private LoseUI source;

        public override void Install(IContainerBuilder builder)
        {
            builder.RegisterInstance(source).AsSelf();
        }
    }
}
