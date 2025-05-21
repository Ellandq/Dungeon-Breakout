using UI;
using UI.Views;

namespace GameStates
{
    public class GameOverState : IGameState
    {
        public string Name => "GameOverState";
        public GameStateStatus Status { get; private set; }
        
        public void Enter()
        {
            Status = GameStateStatus.Entering;
            
            // UI
            UIManager.ActivateView(UIViews.GameOver);
            
            Status = GameStateStatus.Active;
        }

        public void Update()
        {
            
        }

        public void Exit()
        {
            Status = GameStateStatus.Exiting;
            UIManager.DeactivateView(UIViews.GameOver);
            Status = GameStateStatus.Done;
        }

        public bool CanExit()
        {
            return Status == GameStateStatus.Done;
        }
    }
}