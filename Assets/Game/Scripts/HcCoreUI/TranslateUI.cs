using System;
using System.Linq;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GeneralUI.Components
{

    public class TranslateUI : MonoBehaviour, IViewUI
    {
        public float Delay => delay;
        public float Duration => duration;
        public bool IgnoreDuration => ignoreDuration;

        public bool HasModeDefault => mode.Equals(Mode.Default);

        public bool HasModePath => mode.Equals(Mode.Path);

        [BoxGroup("General"), SerializeField] private float delay;
        [BoxGroup("General"), SerializeField] private bool ignoreDuration;

        [BoxGroup("Property"), SerializeField] private Ease easeCurve = Ease.Linear;

        [BoxGroup("Property"), SerializeField] private Mode mode = Mode.Default;

        [BoxGroup("Property"), SerializeField] private float duration = 1f;

        [BoxGroup("Property"), ShowIf(nameof(HasModePath)), SerializeField]
        private Vector2[] path;

        [BoxGroup("Property"), ShowIf(nameof(HasModeDefault)), SerializeField]
        private Vector2 start;

        [BoxGroup("Property"), ShowIf(nameof(HasModeDefault)), SerializeField]
        private Vector2 finish;

        private Vector2 localPosition;


        private Sequence sequence;

        [SerializeField] private bool PlayOnStart = false;
        
        public void Start()
        {
            if (PlayOnStart)
            {
                Show();
            }
        }

        public void Show()
        {
            Play();
        }

        public void Rewind()
        {
            Play(false);
        }

        public void Hide()
        {
            if (sequence != null)
            {
                sequence.Kill();
                DOTween.Kill(sequence);
            }
        }

        private void Play(bool playForward = true)
        {
            if (localPosition == Vector2.zero)
                localPosition = transform.localPosition;

            // Killed together with the object on a scene reload
            sequence = DOTween.Sequence().SetLink(gameObject);
            sequence.SetEase(easeCurve);
            sequence.AppendInterval(delay);

            switch (mode)
            {
                case Mode.Default:
                    DoDefault();
                    return;
                case Mode.Path:
                    DoPath();
                    return;
            }

            void DoDefault()
            {
                Vector2 tmpStart = playForward ? start : finish;
                Vector2 tmpFinish = playForward ? finish : start;

                transform.localPosition = localPosition + tmpStart;
                sequence.Append(transform.DOLocalMove(localPosition + tmpFinish, duration));
            }

            void DoPath()
            {
                Vector2[] tmpPath = playForward ? path : path.Reverse().ToArray();
                transform.localPosition = localPosition + tmpPath[0];

                float childTime = duration / (tmpPath.Length - 1);

                for (int i = 1; i < path.Length; i++)
                    sequence.Append(transform.DOLocalMove(localPosition + tmpPath[i], childTime));
            }
        }

        public enum Mode
        {
            Default = 0,
            Path = 1
        }
    }
}