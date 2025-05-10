namespace GameStates
{
    public interface IGameState
    {
        string Name { get; }
        GameStateStatus Status { get; }
        void Enter();
        void Update();
        void Exit();
        bool CanExit();
    }
}