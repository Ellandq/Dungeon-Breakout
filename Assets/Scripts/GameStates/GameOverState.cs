using UI;
using UI.Views;

namespace GameStates
{
    public class GameOverState : IGameState
    {
        public string Name { get; private set; }
        public GameStateStatus Status { get; private set; }
        
        public void Enter()
        {
            Status = GameStateStatus.Entering;
            Name = "GameOverState";
            UIManager.ActivateView(UIViews.GameOver);
            Status = GameStateStatus.Active;
        }

        public void Update()
        {
            
        }

        public void Exit()
        {
            UIManager.DeactivateView(UIViews.GameOver);
        }

        public bool CanExit()
        {
            return true;
        }
    }
}