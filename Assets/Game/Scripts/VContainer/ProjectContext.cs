using System.Collections.Generic;
using VContainer;
using VContainer.Unity;

namespace Core.VContainer
{
    public class ProjectContext : LifetimeScope
    {
        public List<BaseInstaller> Installers;
        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);
            
            foreach (var installer in Installers)
            {
                installer?.Install(builder);
            }
        }
    }
}