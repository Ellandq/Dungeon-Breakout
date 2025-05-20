using System;
using System.Collections;
using GameStates;
using Input;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using World;

namespace UI.Views
{
    public class GameOverView : UIView
    {
        [Header("Settings")] 
        private const float StartingScale = 70f;
        
        [Header("Object References")]
        [SerializeField] private RectTransform gameOverRectTransform;
        [SerializeField] private Image backgroundImage;
        [SerializeField] private TMP_Text pressAnyButtonText;

        [Header("Events")] 
        private Action _onAnyButtonPressed;

        public override void ActivateView()
        {
            _onAnyButtonPressed = () =>
            {
                UIManager.DeactivateView(UIViews.GameOver);
                GameManager.ChangeState(new RestartGameState());
            };
            
            gameOverRectTransform.localScale = Vector3.one * StartingScale;

            var color = backgroundImage.color;
            color.a = 0f;
            backgroundImage.color = color;

            pressAnyButtonText.alpha = 0f;
            
            gameObject.SetActive(true);

            StartCoroutine(GameOverAnimation());
        }

        public override void DeactivateView()
        {
            StopAllCoroutines();
            InputManager.GetKeyboardInputHandle().RemoveListenerOnAnyKeyPressed(_onAnyButtonPressed);
            gameObject.SetActive(false);
            
        }

        private IEnumerator GameOverAnimation()
        {
            const float duration = 1.0f;
            var elapsed = 0f;
            var initialScale = Vector3.one * StartingScale;
            var targetScale = Vector3.one;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                var t = Mathf.Clamp01(elapsed / duration);

                gameOverRectTransform.localScale = Vector3.Lerp(initialScale, targetScale, t);

                yield return null;
            }
            
            gameOverRectTransform.localScale = Vector3.one;
            StartCoroutine(BackgroundFadeIn());
        }

        private IEnumerator BackgroundFadeIn()
        {
            const float duration = 1.0f;
            var elapsed = 0f;

            var color = backgroundImage.color;
            color.a = 0f;
            backgroundImage.color = color;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                var t = Mathf.Clamp01(elapsed / duration);

                color.a = Mathf.Lerp(0f, 1f, t);
                backgroundImage.color = color;

                yield return null;
            }

            color.a = 1f;
            backgroundImage.color = color;
            StartCoroutine(TextFadeInAndOut());
            WorldManager.Instance.StopAllActors();
        }

        private IEnumerator TextFadeInAndOut()
        {
            InputManager.GetKeyboardInputHandle().AddListenerOnAnyKeyPressed(_onAnyButtonPressed);
            
            const float duration = 1.5f;
            var alpha = pressAnyButtonText.alpha;
            var fadeIn = true;
            var firstFadeIn = true;
            var elapsed = 0f;
            
            while (true)
            {
                elapsed += Time.deltaTime;
                var t = Mathf.Clamp01(elapsed / (firstFadeIn ? duration * 2f : duration));
                fadeIn = fadeIn switch
                {
                    true when Mathf.Approximately(alpha, 1f) => false,
                    false when Mathf.Approximately(alpha, 0.5f) => true,
                    _ => fadeIn
                };
                if (fadeIn)
                {
                    if (firstFadeIn)
                    {
                        pressAnyButtonText.alpha = Mathf.Lerp(0f, 1f, t);
                        yield return null;
                        continue;
                    }

                    pressAnyButtonText.alpha = Mathf.Lerp(0.5f, 1f, t);
                    yield return null;
                    continue;
                }
                firstFadeIn = false;
                pressAnyButtonText.alpha = Mathf.Lerp(1f, 0.5f, t);
                yield return null;
            }
        }

    }
}
