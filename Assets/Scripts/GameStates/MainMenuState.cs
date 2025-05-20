namespace GameStates
{
    public class MainMenuState : IGameState
    {
        public string Name { get; private set; }
        public GameStateStatus Status { get; private set; }
        
        public void Enter()
        {
            Status = GameStateStatus.Entering;
            Name = "MainMenuState";
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