using UI;
using UI.Overlays;
using UI.Views;
using UnityEngine;

namespace GameStates
{
    public class LoadToMenuState : IGameState
    {
        public string Name => "LoadToMenu";
        public GameStateStatus Status { get; private set; }
        
        public void Enter()
        {
            Status = GameStateStatus.Entering;
            
            // TIME
            Time.timeScale = 0f;
            
            // UI
            UIManager.ActivateView(UIViews.Loading, false);
            UIManager.ActivateOverlay(OverlayType.Vignette);
            
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