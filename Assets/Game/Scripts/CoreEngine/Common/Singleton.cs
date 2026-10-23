using UnityEngine;

namespace Common
{
    public class SingletonBase<T> : MonoBehaviour where T : SingletonBase<T>
    {
        protected static T instance { get; private set; }

        // ReSharper disable once StaticMemberInGenericType
        protected static bool Initialized;

        private bool initialized;

        protected virtual void Awake()
        {
            if (instance == (T)this)
                return;
            if (instance == null)
                SetInstance(this as T);
            else
            {
                Destroy(this);
                Debug.LogWarning($"Trying to instantiate second instance of {typeof(T)}");
            }
        }

        protected virtual void OnDestroy()
        {
            if (instance == (T)this)
                SetInstance(null);
        }


        /// <summary>
        ///     Called on awake or before instance returned, depending on what happened first
        /// </summary>
        protected virtual void Init()
        {
            initialized = true;
        }

        protected static void SetInstance(T value)
        {
            instance = value;
            Initialized = value != null;
            if (Initialized && !value.initialized)
                value.Init();
        }
    }


    public class Singleton<T> : SingletonBase<T> where T : Singleton<T>
    {
        public static T Instance
        {
            get
            {
                if (!Initialized)
                    SetInstance(FindObjectOfType<T>());

                return instance;
            }
        }
    }

    [AddComponentMenu("")]
    public class SingletonAuto<T> : SingletonBase<T>
        where T : SingletonAuto<T>
    {
        public static T Instance
        {
            get
            {
                TryInit();
                return instance;
            }
        }

        public static void TryInit()
        {
            if (!Initialized)
            {
                var gameObject = new GameObject(typeof(T).Name);
                DontDestroyOnLoad(gameObject);
                SetInstance(gameObject.AddComponent<T>());
            }
        }
    }
}