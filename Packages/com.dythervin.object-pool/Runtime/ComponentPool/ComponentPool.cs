using System;
using System.Collections.Generic;
using Dythervin.Callbacks;
using Dythervin.Core;
using Dythervin.Core.Utils;
using UnityEngine;
using Object = UnityEngine.Object;
#if ODIN_INSPECTOR && UNITY_EDITOR
using Sirenix.OdinInspector;
#endif

namespace Dythervin.ObjectPool.Component
{
    [Serializable]
    public class ComponentPool<T> : ObjectPoolBase<T>, IComponentPool<T>, IPlayModeListener, ISerializationCallbackReceiver
        where T : UnityEngine.Component
    {
        private static readonly Action<ICallbacks> DestroyedFunc = callbacks =>
        {
            if (!ApplicationExt.IsQuitting)
                Debug.LogWarning($"Pooled object can be destroyed only by pool ({((Object)callbacks).name})");
        };

        private readonly Dictionary<ICallbacks, T> _objectsMap = new();
#if ODIN_INSPECTOR && UNITY_EDITOR
        [HideInEditorMode]
        [ShowInInspector]
        [ReadOnly]
#endif
        private int _instanced;

        private int _instantiated;
#if ODIN_INSPECTOR && UNITY_EDITOR
        [HideInEditorMode]
        [ShowInInspector]
        [ReadOnly]
#endif
        private int _pooled;

        [SerializeField] private bool onlyPoolCanDestroyObj;
        [SerializeField] private T prefab;
        [SerializeField] private Transform parent;

        public bool OnlyPoolCanDestroyObj => onlyPoolCanDestroyObj;

        public int PrefabId { get; private set; }

        public T Prefab => prefab;

        public Transform Parent => parent;

        bool IPlayModeListener.MainThreadOnly => true;

        public ComponentPool(T prefab, Action<T> onGet = null, Action<T> onRelease = null, Action<T> actionOnDestroy = null,
            bool onlyPoolCanDestroyObj = true, bool collectionCheckDefault = DefaultCollectionCheck, int defaultCapacity = DefaultCapacity,
            int maxSize = DefaultMaxSize) : this(onlyPoolCanDestroyObj,
            collectionCheckDefault,
            onGet,
            onRelease,
            actionOnDestroy,
            defaultCapacity,
            maxSize)
        {
            this.prefab = prefab;
        }

        public ComponentPool(bool onlyPoolCanDestroyObj = true, bool collectionCheckDefault = DefaultCollectionCheck,
            Action<T> onGet = null, Action<T> onRelease = null, Action<T> actionOnDestroy = null, int defaultCapacity = DefaultCapacity,
            int maxSize = DefaultMaxSize) : base(collectionCheckDefault, onGet, onRelease, actionOnDestroy, defaultCapacity, maxSize)
        {
            this.onlyPoolCanDestroyObj = onlyPoolCanDestroyObj;
            OnDestroy += Destroy;
            OnRelease += element =>
            {
                GameObject gameObj = element.gameObject;
                if (gameObj.activeSelf)
                    gameObj.SetActive(false);
                gameObj.transform.SetParent(parent, false);
            };

            TryPlayModeSubscribe();
        }

        public override IObjectPool EnsureObjCount(int count)
        {
            bool wasActive = prefab.gameObject.activeSelf;
            if (wasActive)
                prefab.gameObject.SetActive(false);

            base.EnsureObjCount(count);

            if (wasActive)
                prefab.gameObject.SetActive(true);

            return this;
        }

        private void TryPlayModeSubscribe()
        {
            if ((object)prefab != null)
                this.PlayModeSubscribe(true);
        }

        protected ComponentPool() : this(onRelease: null)
        {
        }

        private void AddToPool(T obj)
        {
            if (OnlyPoolCanDestroyObj)
            {
                ICallbacks pooledObject = obj.GetCallbacks();
                pooledObject.OnDestroyed += DestroyedFunc;
                _objectsMap.Add(pooledObject, obj);
            }
        }

        protected T CreateObj(bool setActive)
        {
            if (setActive)
                return GetNew();

            Prefab.gameObject.SetActive(false);
            T obj = GetNew();
            Prefab.gameObject.SetActive(true);
            return obj;
        }

        protected override T CreateNew()
        {
            DAssert.PlayMode();
            T obj = Object.Instantiate(Prefab, Parent);
#if UNITY_EDITOR
            obj.name = obj.name.Replace("(Clone)", $" - {_instantiated.ToString()}");
#endif
            _instantiated++;
            AddToPool(obj);
            return obj;
        }

        protected virtual void OnEnterPlayMode()
        {
            PrefabId = Prefab.GetInstanceID();
            if (Parent == null)
                SetGlobalParent();
        }

        private void Destroy(T obj)
        {
            if (OnlyPoolCanDestroyObj)
            {
                ICallbacks callbacks = obj.GetCallbacks();
                callbacks.OnDestroyed -= DestroyedFunc;
                _objectsMap.Remove(callbacks);
            }

            Object.Destroy(obj);
        }

        protected void SetGlobalParent()
        {
            GameObject go = new GameObject($"{typeof(T).Name} pool")
            {
                isStatic = true,
#if UNITY_EDITOR
                hideFlags = HideFlags.DontSaveInBuild | HideFlags.DontSaveInEditor
#endif
            };

            parent = go.transform;
            Parent.parent = Application.isEditor ? PersistentRoot.Get("ObjectPools").transform : PersistentRoot.Transform;
        }


        public T Get(bool setActive = true)
        {
            T obj;
            while (Stack.TryPop(out obj))
            {
                if (!OnlyPoolCanDestroyObj && !obj)
                    continue;

                if (setActive)
                    obj.gameObject.SetActive(true);

                PoolHelper.Instance.Remove(obj.transform);

                OnGot(obj);
                _instanced++;
                return obj;
            }

            obj = CreateObj(setActive);
            OnGot(obj);
            _instanced++;
            return obj;
        }

        void IPlayModeListener.OnEnterPlayMode()
        {
            OnEnterPlayMode();
        }

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            this.PlayModeSubscribe();
        }
    }
}