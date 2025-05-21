using UI;
using UI.Overlays;
using UI.Views;

namespace GameStates
{
    public class MainMenuState : IGameState
    {
        public string Name => "MainMenuState";
        public GameStateStatus Status { get; private set; }
        
        public void Enter()
        {
            Status = GameStateStatus.Entering;
            
            // UI
            UIManager.DeactivateOverlay(OverlayType.Vignette);
            UIManager.ActivateView(UIViews.MainMenu);
            
            Status = GameStateStatus.Active;
        }

        public void Update()
        {
           
        }

        public void Exit()
        {
            Status = GameStateStatus.Exiting;
            UIManager.DeactivateView(UIViews.MainMenu);
            UIManager.DeactivateView(UIViews.LevelSelect);
            Status = GameStateStatus.Done;
        }

        public bool CanExit()
        {
            return Status == GameStateStatus.Done;
        }
    }
}