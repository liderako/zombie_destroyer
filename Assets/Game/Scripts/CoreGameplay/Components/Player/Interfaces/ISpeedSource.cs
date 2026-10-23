using System;

namespace Gameplay.Components.Player.Interfaces
{
    public interface ISpeedSource
    {
        public event Action<float> UpdateSpeed;
    }
}