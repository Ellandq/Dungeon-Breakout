using System.Collections.Generic;
using Utils;
using UnityEngine;
using Characters;

namespace Characters.Movement.Enemies
{
    public class EnemyController : CustomCharacterController
    {
        [Header("Player Reference")]
        private Characters.Player player;

        [Header("Movement Info")] 
        [SerializeField] private List<Vector3> patrolPositions;
        [SerializeField] private int currentPatrolIndex;
        [SerializeField] private float virtualRotation;
        [SerializeField] private bool isChasing;
        
        public override void Initialize()
        {
            base.Initialize();
            player = PlayerManager.GetPlayer();
            mover.IsMovementEnabled = true;
        }
        
        public override void UpdateMovement()
        {
            var speed = movementSettings.GetSpeed(movementType);
            var directions = Vector2Utils.ConvertFromDirections(moveDirections);
            
            mover.Move(speed * directions);
        }
        
        private void ChangeMoveDirection(MoveDirection direction)
        {
            
        }

        private void ChangeMovementType(MovementType type)
        {
            
        }
    }
}