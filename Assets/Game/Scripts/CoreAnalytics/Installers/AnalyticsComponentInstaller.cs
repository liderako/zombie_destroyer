using Core.VContainer;
using CoreGameplay.Components.Interfaces;
using UnityEngine;
using Utils;
using VContainer;

namespace CoreGameplay.Components.Installers
{
    public class AnalyticsComponentInstaller : BaseInstaller
    {
        [SerializeField] private Ref<IAnalyticsComponent> source;
        
        public override void Install(IContainerBuilder builder)
        {
            builder.RegisterInstance(source.Value);
        }
    }
}