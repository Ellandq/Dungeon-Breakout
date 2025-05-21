using System.Collections.Generic;
using UnityEngine;

namespace UI.Views.Main_Menu.LevelSelectMenu
{
    public class LevelSelect : UIView
    {
        [SerializeField] private ContinueButton continueButton;
        [SerializeField] private Transform contentTransform;
        [SerializeField] private GameObject levelPreviewPrefab;

        [SerializeField] private List<LevelSelectButton> previewButtons;

        private int _lastSetIndex = -1;

        public void UpdateLevelIndex(int index)
        {
            if (_lastSetIndex != -1) previewButtons[_lastSetIndex].SetHighlight(false);
            continueButton.gameObject.SetActive(true);
            _lastSetIndex = index;
            GameManager.SetLevelIndex(index);
        }

        public void GoBack()
        {
            UIManager.DeactivateView(UIViews.LevelSelect);
            UIManager.ActivateView(UIViews.MainMenu);
        }

        public override void ActivateView(bool instant = true)
        {
            base.ActivateView();
            var levelIndex = 0;
            var maxLevelIndex = GameManager.GetCurrentMaxLevel();
            foreach (var preview in previewButtons)
            {
                var lockState = levelIndex > maxLevelIndex;
                preview.SetLockState(lockState);
                levelIndex++;
            }
            if (_lastSetIndex == -1) return;
            continueButton.gameObject.SetActive(false);
            previewButtons[_lastSetIndex].SetHighlight(false);
            _lastSetIndex = -1;
        }
    }
}