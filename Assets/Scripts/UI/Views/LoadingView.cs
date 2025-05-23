﻿using System.Collections;
using GameStates;
using UnityEngine;
using UnityEngine.UI;
using World;

namespace UI.Views
{
    public class LoadingView : UIView
    {
        [Header("Settings")] 
        private const float FinishScale = 90f;
        
        [Header("Object References")]
        [SerializeField] private RectTransform loadingRectTransform;
        [SerializeField] private Image backgroundImage;

        private IGameState _stateToSwitch;

        public override void ActivateView(bool instant = true)
        {
            StopAllCoroutines();
            loadingRectTransform.localScale = Vector3.one;

            var color = backgroundImage.color;
            _stateToSwitch = instant ? new PlayState() : new MainMenuState();
            
            if (instant)
            {
                color.a = 1f;
                backgroundImage.color = color;
            
                gameObject.SetActive(true);

                StartCoroutine(LoadingFinishAnimation());
                return;
            }
            color.a = 0f;
            backgroundImage.color = color;
            
            gameObject.SetActive(true);

            StartCoroutine(LoadingStartAnimation());
        }

        public override void DeactivateView()
        {
            StopAllCoroutines();
            gameObject.SetActive(false);
            GameManager.ChangeState(_stateToSwitch);
        }

        private IEnumerator LoadingFinishAnimation()
        {
            var duration = 1.0f;
            var elapsed = 0f;

            var color = backgroundImage.color;
            color.a = 1f;
            backgroundImage.color = color;

            while (elapsed < duration)
            {
                elapsed += Time.unscaledDeltaTime;
                var t = Mathf.Clamp01(elapsed / duration);

                color.a = Mathf.Lerp(1f, 0f, t);
                backgroundImage.color = color;

                yield return null;
            }

            elapsed = 0f;
            color.a = 0f;
            backgroundImage.color = color;
            duration = 0.5f;
            
            var initialScale = Vector3.one;
            var targetScale= Vector3.one * FinishScale;

            while (elapsed < duration)
            {
                elapsed += Time.unscaledDeltaTime;
                var t = Mathf.Clamp01(elapsed / duration);

                loadingRectTransform.localScale = Vector3.Lerp(initialScale, targetScale, t);

                yield return null;
            }
            
            loadingRectTransform.localScale = Vector3.one * FinishScale;
            DeactivateView();
        }
        
        private IEnumerator LoadingStartAnimation()
        {
            loadingRectTransform.localScale = Vector3.one * FinishScale;
            var color = backgroundImage.color;
            var duration = 0.5f;
            var elapsed = 0f;
            color.a = 0f;
            backgroundImage.color = color;
            
            var initialScale = Vector3.one * FinishScale;
            var targetScale= Vector3.one;

            while (elapsed < duration)
            {
                elapsed += Time.unscaledDeltaTime;
                var t = Mathf.Clamp01(elapsed / duration);

                loadingRectTransform.localScale = Vector3.Lerp(initialScale, targetScale, t);

                yield return null;
            }
            
            loadingRectTransform.localScale = Vector3.one;
            
            duration = 1.0f;
            elapsed = 0f;
            color.a = 0f;
            backgroundImage.color = color;

            while (elapsed < duration)
            {
                elapsed += Time.unscaledDeltaTime;
                var t = Mathf.Clamp01(elapsed / duration);

                color.a = Mathf.Lerp(0f, 1f, t);
                backgroundImage.color = color;

                yield return null;
            }
            
            WorldManager.Instance.DeLoadLevel();
            StartCoroutine(LoadingFinishAnimation());
        }
    }
}
