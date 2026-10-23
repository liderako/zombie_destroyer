using System;
using CoreGameplay.Components;
using DG.Tweening;
using GameUI;
using GeneralUI.Components;
using UnityEngine;
using VContainer;
using Random = UnityEngine.Random;

namespace Components.CoreGameplay.Behavior
{
    public class SpriteFlyBehaviorComponent : MonoBehaviour
    {
        [Inject] private MoneyComponent moneyComponent;
        [SerializeField] private CoinBarUI coinBarUI;
        
        public void AddFlyAnimation(int amount, CounterUI from, CounterUI to, int maxPool, Action onCompleted = null)
        {
            int rewardPart = amount / maxPool;

            moneyComponent.AddCoins(amount);
            moneyComponent.Save();
            
            for (int i = 0; i < maxPool; i++)
            {
                var coin = coinBarUI.GetFreeIconByPool();

                if(i == maxPool - 1)
                    StartFlyAnimation(coin, i, rewardPart, () => { Finish(); });
                else
                    StartFlyAnimation(coin, i, rewardPart);

            }

            void Finish()
            {
                onCompleted?.Invoke();
            }

            void StartFlyAnimation(RectTransform target, int index, int rewardPart, Action onCompleted = null)
            {
                target.position = from.Icon.position;
                target.sizeDelta = from.Icon.sizeDelta;
                target.gameObject.SetActive(true);
                target.localScale = Vector3.zero;

                var startPosition = from.Icon.position + new Vector3(Random.Range(-150f, 150f), Random.Range(-150f, 150f), 0);

                Vector3[] path = new Vector3[3];
                path[0] = startPosition;
                path[2] = to.Icon.position;
                path[1] = (path[2] + path[0]) / 2f;

                path[1].x = target.position.x;

                DOTween.Sequence()
                    .SetLink(target.gameObject)
                    .AppendInterval(0.025f * index)
                    .AppendCallback(() =>
                    {
                        target.GetComponent<ScaleUI>().Show();
                    });

                DOTween.Sequence()
                    .SetLink(target.gameObject)
                    .SetEase(Ease.InCirc)
                    .AppendInterval(0.025f * index)
                    .Append(target.DOMove(startPosition, 0.125f))
                    .Append(target.DOPath(path, 1f, PathType.CatmullRom))
                    .AppendCallback(() =>
                    {
                        target.gameObject.SetActive(false);
                        onCompleted?.Invoke();
                    });

                DOTween.Sequence()
                    .SetLink(target.gameObject)
                    .AppendInterval(0.025f * index)
                    .Append(target.DOSizeDelta(to.Icon.sizeDelta, 1f));
            }
        }
    }
}