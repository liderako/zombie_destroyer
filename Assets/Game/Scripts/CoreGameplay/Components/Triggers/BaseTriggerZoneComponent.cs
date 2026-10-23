using System;
using Common.Interfaces;
using UnityEngine;

namespace Gameplay.Components.Triggers
{
    public class BaseTriggerZoneComponent : MonoBehaviour, ITriggerComponent
    {
        [SerializeField] private bool IsTagCompare;
        [SerializeField] private string[] TagCompare;
        
        [SerializeField] private bool IsLayerCompare;
        [SerializeField] private int[] LayerCompare;
        
        public virtual event Action<bool> TriggerAction;
        public bool IsTrigger { get; set; }
        
        public Transform TriggerObject { get; set; }

        public virtual void OnTriggerEnter(Collider other)
        {
            if (!enabled) return;
            if (IsTrigger) return;
            if (IsTagCompare && !CompareTag(other.gameObject)) return;
            if (IsLayerCompare && !CompareLayer(other.gameObject)) return;
            TriggerObject = other.transform;
            Enter();
        }

        public virtual void OnTriggerExit(Collider other)
        {
            if (!enabled) return;
            if (!IsTrigger) return;
            if (IsTagCompare && !CompareTag(other.gameObject)) return;
            if (IsLayerCompare && !CompareLayer(other.gameObject)) return;
            Exit();
        }

        public bool CompareTag(GameObject obj)
        {
            for (int i = 0; i < TagCompare.Length; i++)
            {
                if (obj.CompareTag(TagCompare[i]))
                {
                    return true;
                }
            }
            return false;
        }

        public bool CompareLayer(GameObject obj)
        {
            for (int i = 0; i < LayerCompare.Length; i++)
            {
                if (obj.layer == LayerCompare[i])
                {
                    return true;
                }
            }
            return false;
        }

        public virtual void Enter()
        {
            IsTrigger = true;
            TriggerAction?.Invoke(true);
        }

        public virtual void Exit()
        {
            IsTrigger = false;
            TriggerObject = null;
            TriggerAction?.Invoke(false);
        }

        public virtual void ForceExit()
        {
            IsTrigger = false;
            TriggerObject = null;
            TriggerAction?.Invoke(false);
        }
    }
}