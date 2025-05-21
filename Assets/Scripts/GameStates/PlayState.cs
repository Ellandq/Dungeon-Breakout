using UnityEngine;

namespace GameStates
{
    public class PlayState : IGameState
    {
        public string Name => "PlayState";
        public GameStateStatus Status { get; private set; }
        
        public void Enter()
        {
            Status = GameStateStatus.Entering;
            Time.timeScale = 1f;
            Status = GameStateStatus.Active;
        }

        public void Update()
        {
            
        }

        public void Exit()
        {
            Status = GameStateStatus.Exiting;
            
            Status = GameStateStatus.Done;
        }

        public bool CanExit()
        {
            return Status == GameStateStatus.Done;
        }
    }
}