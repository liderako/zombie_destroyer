using Core.VContainer;
using CoreGameplay.Components;
using VContainer;

namespace Game.Scripts.CoreGameplay.Components.Installers
{
    public class MoneyComponentInstaller : BaseInstaller
    {
        public override void Install(IContainerBuilder builder)
        {
            builder.RegisterInstance(new MoneyComponent()).AsSelf().AsImplementedInterfaces();
        }
    }
}