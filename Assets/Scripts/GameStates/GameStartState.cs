using UI;
using UI.Views;
using UnityEngine;
using World;

namespace GameStates
{
    public class GameStartState : IGameState
    {
        public string Name => "GameStartState";
        public GameStateStatus Status { get; private set; }
        
        public void Enter()
        {
            Status = GameStateStatus.Entering;
            var levelIndex = GameManager.GetLevelIndex();
            WorldManager.Instance.LoadLevel(levelIndex);
            UIManager.ActivateView(UIViews.Loading);
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