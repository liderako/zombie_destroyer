using System.Collections.Generic;
using Gameplay.Stack;
using UnityEngine;

namespace Game.Scripts.CoreEngine.Stack
{
    public class StackDisappearComponent : MonoBehaviour
    {
        [SerializeField] private List<ItemStack> stacks;

        private bool isPlayed;

        public void Clear()
        {
            foreach (var stack in stacks)
            {
                if (!stack.Empty)
                {
                    stack.Clear();
                }
            }
        }
    }
}