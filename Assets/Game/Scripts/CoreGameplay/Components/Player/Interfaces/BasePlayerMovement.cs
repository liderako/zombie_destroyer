using Unity.Cinemachine;
using UnityEngine;

namespace Gameplay.Components.Player.Interfaces
{
    public abstract class BasePlayerMovement : BaseMovementComponent
    {
        [field:SerializeField] public CinemachineCamera Camera { get; set; }
    }
}