using UnityEngine.UI;

namespace UI.Views.Main_Menu
{
    public class StartButton : MainMenuButton
    {
        private void Awake()
        {
            var button = GetComponent<Button>();
            button.onClick.AddListener(() =>
            {
                mainMenuView.DeactivateView();
                UIManager.ActivateView(UIViews.LevelSelect);
            });
        }
    }
}