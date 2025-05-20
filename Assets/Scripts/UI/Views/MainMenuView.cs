using GameStates;

namespace UI.Views
{
    public class MainMenuView : UIView
    {
        public override void ActivateView()
        {
            gameObject.SetActive(true);
        }
        
        public override void DeactivateView()
        {
            gameObject.SetActive(false);
            GameManager.SetLevelIndex(0);
            GameManager.ChangeState(new GameStartState());
        }
    }
}