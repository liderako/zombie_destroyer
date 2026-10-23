using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Common.Collections
{
    [System.Serializable]
    public class AutoExpandableList<T> where T : Component
    {
        [SerializeField] private List<T> list;
        public event System.Action<T> OnSpawn; 

        public IReadOnlyList<T> List => list;

        public AutoExpandableList(IEnumerable<T> components)
        {
            list = new List<T>(components);
        }

        public void Trim(int lenght)
        {
            for (int i = 0; i < list.Count; i++)
            {
                list[i].gameObject.SetActive(i < lenght);
            }
        }

        public T this[int index]
        {
            get
            {
                PopulateTo(index);
                return list[index];
            }
            set
            {
                PopulateTo(index);
                list[index] = value;
            }
        }

        public void PopulateTo(int index)
        {
            if (list.Count > index)
                return;

            T obj = list[0];
            Transform parent = obj.transform.parent;
            while (list.Count <= index)
            {
                T newObj = Object.Instantiate(obj, parent);
                OnSpawn?.Invoke(newObj);
                list.Add(newObj);
            }
        }
    }
}