using World;

namespace GameStates
{
    public class GameStartState : IGameState
    {
        public string Name { get; private set; }
        public GameStateStatus Status { get; private set; }
        
        public void Enter()
        {
            Status = GameStateStatus.Entering;
            Name = "GameStartState";
            
            var levelIndex = GameManager.GetLevelIndex();
            WorldManager.Instance.LoadLevel(levelIndex);
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