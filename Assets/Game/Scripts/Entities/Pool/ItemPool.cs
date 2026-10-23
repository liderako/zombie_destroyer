using System;
using Common;
using Dythervin.Callbacks;
using Dythervin.ObjectPool.Component;
using Dythervin.ObjectPool.Component.Global;
using UnityEngine;

namespace PencilFlow.Entities
{
    public class ItemPool : Singleton<ItemPool>
    {
        // public IComponentPool<EditorCell> CellObjectPool => cell.Pool;
        
        private static readonly Action<ICallbacks> CallbacksOnOnDisabled;
        
        static ItemPool()
        {
            CallbacksOnOnDisabled = OnCallbacksOnOnDisabled;
        }
        
        protected override void Init()
        {
            base.Init();
            
            // CellObjectPool.EnsureObjCount(500);
            // VfxHitPool.OnCreated += OnVfxPoolFloorOnOnCreated;
            // VfxHitPool.EnsureObjCount(150);
        }
        
        protected override void OnDestroy()
        {
            // VfxHitPool.OnCreated -= OnVfxPoolFloorOnOnCreated;
            base.OnDestroy();
        }
        
        private void OnVfxPoolFloorOnOnCreated(ParticleSystem vfx)
        {
            var callbacks = vfx.GetCallbacks();
            callbacks.OnDisabled += CallbacksOnOnDisabled;
        }
        
        static void OnCallbacksOnOnDisabled(ICallbacks callbacks)
        {
            GlobalPools.ReleaseOrDestroy(callbacks.gameObject);
        }
    }
}