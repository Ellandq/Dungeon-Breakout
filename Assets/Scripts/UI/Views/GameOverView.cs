using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Views
{
    public class GameOverView : UIView
    {
        [Header("Settings")] 
        private const float StartingTopPos = 11500f;
        private const float StartingBottomPos = -11500f;
        private const float StartingScale = 70f;
        
        [Header("Object References")]
        [SerializeField] private RectTransform gameOverRectTransform;
        [SerializeField] private Image backgroundImage;

        public override void ActivateView()
        {
            gameOverRectTransform.offsetMin = new Vector2(gameOverRectTransform.offsetMin.x, StartingBottomPos);
            gameOverRectTransform.offsetMax = new Vector2(gameOverRectTransform.offsetMax.x, StartingTopPos);
            gameOverRectTransform.localScale = Vector3.one * StartingScale;

            var color = backgroundImage.color;
            color.a = 0f;
            backgroundImage.color = color;
            
            gameObject.SetActive(true);

            StartCoroutine(GameOverAnimation());
        }

        public override void DeactivateView()
        {
            gameObject.SetActive(false);
        }

        private IEnumerator GameOverAnimation()
        {
            const float duration = 2.0f;
            var elapsed = 0f;

            var initialOffsetMin = new Vector2(gameOverRectTransform.offsetMin.x, StartingBottomPos);
            var initialOffsetMax = new Vector2(gameOverRectTransform.offsetMax.x, StartingTopPos);
            var initialScale = Vector3.one * StartingScale;

            var targetOffsetMin = new Vector2(initialOffsetMin.x, 0f);
            var targetOffsetMax = new Vector2(initialOffsetMax.x, 0f);
            var targetScale = Vector3.one;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                var t = Mathf.Clamp01(elapsed / duration);

                var newOffsetMinY = Mathf.Lerp(initialOffsetMin.y, targetOffsetMin.y, t);
                var newOffsetMaxY = Mathf.Lerp(initialOffsetMax.y, targetOffsetMax.y, t);
                gameOverRectTransform.offsetMin = new Vector2(gameOverRectTransform.offsetMin.x, newOffsetMinY);
                gameOverRectTransform.offsetMax = new Vector2(gameOverRectTransform.offsetMax.x, newOffsetMaxY);

                gameOverRectTransform.localScale = Vector3.Lerp(initialScale, targetScale, t);

                yield return null;
            }

            gameOverRectTransform.offsetMin = new Vector2(gameOverRectTransform.offsetMin.x, 0f);
            gameOverRectTransform.offsetMax = new Vector2(gameOverRectTransform.offsetMax.x, 0f);
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
        }

    }
}
