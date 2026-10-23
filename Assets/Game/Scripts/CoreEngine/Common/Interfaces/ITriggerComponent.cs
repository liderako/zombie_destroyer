using System;

namespace Common.Interfaces
{
    public interface ITriggerComponent
    {
        public event Action<bool> TriggerAction;
        public bool IsTrigger { get; set; }
    }
}