using UI;
using UI.Overlays;
using UI.Views;
using UnityEngine;
using World;

namespace GameStates
{
    public class GameLoadState : IGameState
    {
        public string Name => "GameStartState";
        public GameStateStatus Status { get; private set; }
        
        public void Enter()
        {
            Status = GameStateStatus.Entering;
            
            // TIME
            Time.timeScale = 0f;
            
            // UI
            UIManager.ActivateView(UIViews.Loading);
            UIManager.ActivateOverlay(OverlayType.Vignette);
            
            // WORLD
            var levelIndex = GameManager.GetLevelIndex();
            WorldManager.Instance.LoadLevel(levelIndex);
            
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