using System;
using Common;
using Dythervin.Callbacks;
using Dythervin.ObjectPool.Component;
using Dythervin.ObjectPool.Component.Global;
using Gameplay.Stack.Item;
using UnityEngine;

namespace Gameplay.Stack.Pool
{
    public class ItemPool : Singleton<ItemPool>
    {
        public IComponentPool<ItemBase> ObjectPool => globalPooled.Pool;
        public IComponentPool<ParticleSystem> VfxHitPool => vfxHitPool.Pool;
        public IComponentPool<ParticleSystem> VfxCollect => vfxCollectPool.Pool;
        
        [SerializeField] private PrefabPooled<ItemBase> globalPooled;
        [SerializeField] private PrefabPooled<ParticleSystem> vfxHitPool;
        [SerializeField] private PrefabPooled<ParticleSystem> vfxCollectPool;
        private static readonly Action<ICallbacks> CallbacksOnOnDisabled;
        
        static ItemPool()
        {
            CallbacksOnOnDisabled = OnCallbacksOnOnDisabled;
        }
        
        protected override void Init()
        {
            base.Init();
        
            VfxHitPool.OnCreated += OnVfxPoolFloorOnOnCreated;
            VfxCollect.OnCreated += OnVfxPoolFloorOnOnCreated;
            
            
            ObjectPool.EnsureObjCount(500);
            VfxHitPool.EnsureObjCount(150);
            VfxCollect.EnsureObjCount(200);
            
        }
        
        protected override void OnDestroy()
        {
            VfxHitPool.OnCreated -= OnVfxPoolFloorOnOnCreated;
            VfxCollect.OnCreated -= OnVfxPoolFloorOnOnCreated;
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