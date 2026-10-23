using CoreGameplay.Components;
using DG.Tweening;
using GeneralUI.Components;
using UnityEngine;
using VContainer;

namespace GameUI
{
    public class CoinBarUI : BaseCanvasUI
    {
        [Inject] private MoneyComponent moneyComponent;
        public CounterUI CounterBar => counter;

        [SerializeField] private CounterUI counter;

        [SerializeField] private Transform poolHolder;

        [SerializeField] private GameObject PrefabCoin;

        private int lastValue;

        private void Awake()
        {
            CreatePool();
        }

        private void Start()
        {
            moneyComponent.Load();
            counter.Set(moneyComponent.Value);
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            moneyComponent.UpdateCurrencyValue += OnHandleRefresh;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            moneyComponent.UpdateCurrencyValue -= OnHandleRefresh;
        }

        private void OnHandleRefresh(int newValue)
        {
            counter.LerpPlay(lastValue, newValue);
        }

        public void UnscaleUI()
        {
            counter.UnscaleUI();
        }

        public void StartScaleAnimation()
        {
            counter.StartScaleAnimation();
        }

        private void CreatePool()
        {
            for (int i = 0; i < MaxPoolSize(); i++)
            {
                var coin = Instantiate(PrefabCoin, poolHolder);
                coin.gameObject.SetActive(false);
            }
        }

        public RectTransform GetFreeIconByPool()
        {
            for (int i = 0; i < poolHolder.childCount; i++)
            {
                var coin = poolHolder.GetChild(i);

                if(!coin.gameObject.activeSelf)
                    return coin as RectTransform;
            }
            return null;
        }

        public void ResetPool()
        {
            for (int i = 0; i < poolHolder.childCount; i++)
            {
                if(poolHolder.GetChild(i).gameObject.activeSelf)
                {
                    poolHolder.GetChild(i).DOKill();
                    poolHolder.GetChild(i).gameObject.SetActive(false);
                }
            }
        }

        private int MaxPoolSize() => 50;
    }
}
