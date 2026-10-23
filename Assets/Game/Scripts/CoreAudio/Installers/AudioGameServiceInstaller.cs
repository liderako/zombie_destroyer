using Core.VContainer;
using UnityEngine;
using VContainer;

namespace CoreAudio
{
    public class AudioGameServiceInstaller : BaseInstaller
    {
        [SerializeField] private AudioGameService source;
        
        public override void Install(IContainerBuilder builder)
        {
            builder.RegisterInstance(source).AsImplementedInterfaces();
        }
    }
}