using UI;
using UI.Views;
using UnityEngine;
using World;

namespace GameStates
{
    public class RestartGameState : IGameState
    {
        public string Name => "GameRestartState";
        public GameStateStatus Status { get; private set; }
        
        public void Enter()
        {
            Status = GameStateStatus.Entering;
            Time.timeScale = 0f;
            UIManager.ActivateView(UIViews.Loading);
            WorldManager.Instance.RestartCurrentLevel();
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