using System.Collections;
using GameStates;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Views.Main_Menu
{
    public class ContinueButton : MainMenuButton
    {
        [SerializeField] private TMP_Text additionalText;
        private int _currentLevelIndex;

        private void Awake()
        {
            var button = GetComponent<Button>();
            button.onClick.AddListener(() =>
            {
                mainMenuView.DeactivateView();
            });
        }
        
        public override void OnPointerEnter(PointerEventData eventData)
        {
            StopAllCoroutines();
            _currentLevelIndex = GameManager.GetLevelIndex() + 1;
            additionalText.text = $"- Level {_currentLevelIndex}";
            StartCoroutine(AdditionalTextFadeIn());
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            StopAllCoroutines();
            StartCoroutine(AdditionalTextFadeOut());
        }

        private IEnumerator AdditionalTextFadeIn()
        {
            var fadeInDuration = .5f * (1f - additionalText.alpha);
            var elapsed = 0f;

            while (elapsed < fadeInDuration)
            {
                elapsed += Time.unscaledDeltaTime;
                var t = Mathf.Clamp01(elapsed / fadeInDuration);
                additionalText.alpha = Mathf.Lerp(0f, 1f, t);
                yield return null;
            }

            additionalText.alpha = 1f;
        }
        
        private IEnumerator AdditionalTextFadeOut()
        {
            var fadeOutDuration = .5f * additionalText.alpha;
            var elapsed = 0f;

            while (elapsed < fadeOutDuration)
            {
                elapsed += Time.unscaledDeltaTime;
                var t = Mathf.Clamp01(elapsed / fadeOutDuration);
                additionalText.alpha = Mathf.Lerp(1f, 0f, t);
                yield return null;
            }
            
            additionalText.alpha = 0f;
        }
    }
}