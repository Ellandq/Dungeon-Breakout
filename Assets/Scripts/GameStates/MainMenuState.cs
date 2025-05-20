namespace GameStates
{
    public class MainMenuState : IGameState
    {
        public string Name => "MainMenuState";
        public GameStateStatus Status { get; private set; }
        
        public void Enter()
        {
            Status = GameStateStatus.Entering;
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