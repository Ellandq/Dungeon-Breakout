using UI;
using UI.Views;
using UnityEngine;

namespace GameStates
{
    public class YouWonGameState : IGameState
    {
        public string Name => "GameWonState";
        public GameStateStatus Status { get; private set; }
        
        public void Enter()
        {
            Status = GameStateStatus.Entering;
            UIManager.ActivateView(UIViews.YouWon);
            Time.timeScale = 0f;
            Status = GameStateStatus.Active;
        }

        public void Update()
        {
            
        }

        public void Exit()
        {
            
        }

        public bool CanExit()
        {
            return Status == GameStateStatus.Active;
        }
    }
}