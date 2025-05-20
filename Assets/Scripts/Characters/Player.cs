using GameStates;
using UnityEngine;

namespace Characters
{
    public class Player : Characters
    {
        [Header("Object References")]
        [SerializeField] private CircleCollider2D trigger;
        
        [Header("Settings")]
        [SerializeField] private LayerMask layerMask;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            var objLayer = other.gameObject.layer;
            if (((1 << objLayer) & layerMask) == 0) return;
            if (objLayer == LayerMask.NameToLayer("Enemy"))
            {
                controller.DisableMovement();
                GameManager.ChangeState(new GameOverState());
            }
            else if (objLayer == LayerMask.NameToLayer("Exit"))
            {
                controller.DisableMovement();
                GameManager.ChangeState(new YouWonGameState());
            }
            else if (objLayer == LayerMask.NameToLayer("Panel"))
            {
                // controller.DisableMovement();
                // TODO
            }
        }
    }
}