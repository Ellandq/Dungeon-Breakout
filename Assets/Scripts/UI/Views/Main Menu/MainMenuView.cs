using GameStates;
using UI.Overlays;

namespace UI.Views
{
    public class MainMenuView : UIView
    {
        public override void ActivateView()
        {
            gameObject.SetActive(true);
            UIManager.DeactivateOverlay(OverlayType.Vignette);
        }
        
        public override void DeactivateView()
        {
            gameObject.SetActive(false);
            GameManager.SetLevelIndex(0);
            UIManager.ActivateOverlay(OverlayType.Vignette);
            GameManager.ChangeState(new GameStartState());
        }
    }
}