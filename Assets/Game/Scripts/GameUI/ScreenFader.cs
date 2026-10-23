using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GameUI
{
    public class ScreenFader : MonoBehaviour
    {
        [SerializeField] private Image fadeOverlay;

        public void Awake()
        {
            SceneManager.sceneLoaded += (scene, mode) => FadeOut();
        }

        public void FadeIn(Action callBack = null)
        {
            fadeOverlay.enabled = true;
            fadeOverlay.color = Color.clear;
            fadeOverlay.DOColor(Color.black, 0.5f).OnComplete(() => callBack?.Invoke());
        }

        public void FadeOut(Action callBack = null)
        {
            fadeOverlay.color = Color.black;
            fadeOverlay.DOColor(Color.clear, 0.5f).OnComplete(() => callBack?.Invoke());
        }
    }
}
