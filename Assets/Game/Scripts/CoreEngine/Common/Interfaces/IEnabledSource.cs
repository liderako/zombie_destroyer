using System;

namespace Common.Interfaces
{
    public interface IEnabledSource
    {
        public event Action EnableAction;
        public event Action DisableAction;
    }
}