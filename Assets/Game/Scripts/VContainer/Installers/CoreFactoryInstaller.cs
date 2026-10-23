using Core.VContainer;
using VContainer;

namespace Core.Installers
{
    public class CoreFactoryInstaller : BaseInstaller
    {
        public override void Install(IContainerBuilder builder)
        {
            builder.Register<VContainerFactory>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
        }
    }
}