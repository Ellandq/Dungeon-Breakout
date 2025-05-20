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
            
        }

        public bool CanExit()
        {
            return true;
        }
    }
}