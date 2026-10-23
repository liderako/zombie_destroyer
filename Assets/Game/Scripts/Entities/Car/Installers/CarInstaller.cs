using Core.VContainer;
using UnityEngine;
using VContainer;

namespace Entities.Car.Installers
{
    public class CarInstaller : BaseInstaller
    {
        [SerializeField] private CarComponent source;

        public override void Install(IContainerBuilder builder)
        {
            builder.RegisterInstance(source).AsSelf();
        }
    }
}
