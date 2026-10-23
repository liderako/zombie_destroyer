using Core.VContainer;
using UnityEngine;
using VContainer;

namespace CoreAudio
{
    public class SoundSettingsInstaller : BaseInstaller
    {
        [SerializeField] private SoundSettings source;
        
        public override void Install(IContainerBuilder builder)
        {
            builder.RegisterInstance(source).AsImplementedInterfaces();
        }  
    }
}