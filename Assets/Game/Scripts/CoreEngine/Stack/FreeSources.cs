using System.Collections.Generic;
using Common;
using Gameplay.Stack.Interactions;
using UnityEngine;
using Utils;

namespace Gameplay.Stack
{
    public class FreeSources : Singleton<FreeSources>
    {
        [SerializeField] private Ref<IItemReceiver>[] stacks;

        public IReadOnlyList<IItemReceiver> Values { get; private set; }

        protected override void Init()
        {
            base.Init();
            var array = new IItemReceiver[stacks.Length];
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = stacks[i].Value;
            }

            Values = array;
        }
    }
}