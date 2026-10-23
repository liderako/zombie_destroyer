using Core.VContainer;
using GameUI;
using UnityEngine;
using VContainer;

namespace GameUI.Installers
{
    public class CoinBarUIInstaller : BaseInstaller
    {
        [SerializeField] private CoinBarUI source;

        public override void Install(IContainerBuilder builder)
        {
            builder.RegisterInstance(source).AsSelf();
        }
    }
}
