using UnityEngine;

namespace GameStates
{
    public class PlayState : IGameState
    {
        public string Name { get; private set; }
        public GameStateStatus Status { get; private set; }
        
        public void Enter()
        {
            Status = GameStateStatus.Entering;
            Name = "PlayState";
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