using Core.VContainer;
using Memento;
using UnityEngine;
using VContainer;

namespace CoreGameplay.Components.Installers
{
    public class LevelManagerInstaller : BaseInstaller
    {
        [SerializeField] private LevelManager source;
        
        public override void Install(IContainerBuilder builder)
        {
            Application.targetFrameRate = 60;
            MementoSystem.CacheFile();
            source.Load();
            builder.RegisterInstance(source).AsSelf();
        }
    }
}