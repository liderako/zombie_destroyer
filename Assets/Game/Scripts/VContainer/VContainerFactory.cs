using System;
using System.Collections.Generic;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Core.VContainer
{
    public class VContainerFactory: IDisposable
    {
        private readonly IObjectResolver resolver;
        private readonly List<IDisposable> disposables = new();
    
        public VContainerFactory(IObjectResolver resolver)
        {
            this.resolver = resolver;
        }

        public GameObject Create(GameObject prefab, Vector3 position)
        {
            var instance = resolver.Instantiate(prefab, position, Quaternion.identity);
            resolver.Inject(instance);
            return instance;
        }
        
        public T CreateComponent<T>(T prefab, Vector3 position) where T : Component
        {
            var instance = resolver.Instantiate(prefab, position, Quaternion.identity);
            resolver.Inject(instance);
            return instance;
        }
    
        public T Inject<T>(T instance) where T : IDisposable
        {
            if (instance == null)
                throw new ArgumentNullException(nameof(instance));
            resolver.Inject(instance);
            disposables.Add(instance);
            return instance;
        }

        public void Dispose<T>(T instance) where T : IDisposable
        {
            if (instance == null)
                return;
            if (disposables.Remove(instance))
                instance.Dispose();
        }
    
        public void DisposeAll()
        {
            for (int i = disposables.Count - 1; i >= 0; i--)
            {
                disposables[i].Dispose();
            }
            disposables.Clear();
        }

        public void Dispose()
        {
            DisposeAll();
        }
    }
}