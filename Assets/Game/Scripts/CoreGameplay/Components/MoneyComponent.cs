using System;
using Memento;
using Memento.Interfaces;

namespace CoreGameplay.Components
{
    [Serializable]
    public class MoneyComponent : IMementoComponent
    {
        public int Value { get; private set; }

        public event Action<int> UpdateCurrencyValue; // bool used for bounce in UI

        public void SetCoins(int v)
        {
            Value = v;
            UpdateCurrencyValue?.Invoke(Value);
        }

        public void AddCoins(int coins)
        {
            Value += coins;
            UpdateCurrencyValue?.Invoke(Value);
        }

        public bool IsEnoughMoney(int value)
        {
            return Value >= value;
        }

        public void SubMoney(int value)
        {
            Value -= value;
            UpdateCurrencyValue?.Invoke(Value);
        }

        public void Save()
        {
            MementoSystem.Save("money", Value);
        }

        public void Load()
        {
            Value = MementoSystem.Load("money", 0);
        }
    }
}