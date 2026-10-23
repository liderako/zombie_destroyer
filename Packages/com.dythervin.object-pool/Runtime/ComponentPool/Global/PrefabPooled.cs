using System;
using Dythervin.Core;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;
#if UNITY_EDITOR && ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace Dythervin.ObjectPool.Component.Global
{
    [Serializable]
    public sealed class PrefabPooled<T>
        where T : UnityEngine.Component
    {
#if ODIN_INSPECTOR && UNITY_EDITOR
        [Required]
#endif
        [SerializeField] private T value;

#if ODIN_INSPECTOR && UNITY_EDITOR
        [ShowInInspector] [ReadOnly]
#endif
        internal int instanceId;

        private IComponentPool<T> _pool;

        public IComponentPool<T> Pool
        {
            get
            {
                if (_pool == null)
                {
                    instanceId = value.GetInstanceID();
                    _pool = this.GetPool();
                }

                return _pool;
            }
        }

        public T Prefab => value;

        public PrefabPooled([NotNull] T prefab)
        {
            value = prefab;
        }

        /// <summary>
        /// Can be called in editor mode
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        public T GetSafe(Transform parent)
        {
#if UNITY_EDITOR
            if (!ApplicationExt.IsPlaying)
                return (T)PrefabUtility.InstantiatePrefab(Prefab, parent);
#endif
            return Pool.Get(parent);
        }
    }
}