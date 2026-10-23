using UnityEngine.UI;
using TMPro;
using UnityEngine;
using DG.Tweening;

namespace GeneralUI.Components
{
    public class TaskUI : MonoBehaviour
    {
        public Image Icon => icon;

        [SerializeField] private Image icon;
        [SerializeField] private TextMeshProUGUI numberText;
        [SerializeField] private Image checkMark;
        [SerializeField] private Image backCheckMark;
        private int numberOfItems = 0;

        public Image SetTaskIcon(Image icon)
        {
            icon.sprite = icon.sprite;
            IncrementNumber();
            return icon;
        }

        public void IncrementNumber()
        {
            numberOfItems++;
            numberText.text = numberOfItems.ToString();
        }

        private void DecrementNumber()
        {
            numberOfItems--;
            if (numberOfItems < 1)
                return;

            numberText.text = numberOfItems.ToString();
        }

        private void CompleteAnimation()
        {
            Sequence seq = DOTween.Sequence();
            seq.Append(transform.DOScale(1, 0.3f).SetEase(Ease.OutBack));
            /*seq.AppendCallback(() =>
            {
                numberText.gameObject.SetActive(false);
                checkMark.gameObject.SetActive(true);
            });*/
            seq.Append(transform.DOScale(0.9f, 0.3f).SetEase(Ease.OutBack));
        }

        public void CollectIcon(Image icon)
        {
            if (icon.sprite == icon.sprite)
            {
                transform.DOJump(transform.position, 5, 1, 0.5f).SetEase(Ease.OutBack);
                DecrementNumber();
                if (numberOfItems == 0)
                {
                    Sequence seq = DOTween.Sequence();
                    seq.Append(
                        transform.DORotate(Vector3.up * 90, 0.15f, RotateMode.FastBeyond360).SetEase(Ease.Linear));
                    seq.AppendCallback(() =>
                    {
                        backCheckMark.gameObject.SetActive(true);
                        icon.gameObject.SetActive(false);
                    });
                    seq.Append(transform.DORotate(Vector3.up * 180, 0.15f, RotateMode.FastBeyond360)
                        .SetEase(Ease.Linear));

                    seq.OnComplete(() => CompleteAnimation());
                }
            }
        }
    }
}