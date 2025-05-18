using System.Collections.Generic;
using UnityEngine;

namespace Characters.Movement
{
    public abstract class CustomCharacterController : MonoBehaviour, ICharacterController
    {
        [Header("Character Mover")]
        [SerializeField] protected CharacterMover mover;
        
        [Header("Movement Info")]
        protected Dictionary<MoveDirection, bool> moveDirections;
        [SerializeField] protected MovementSettings movementSettings;
        [SerializeField] protected MovementType movementType;
        
        public virtual void Initialize()
        {
            moveDirections = new Dictionary<MoveDirection, bool>{
                { MoveDirection.Up, false },
                { MoveDirection.Down, false },
                { MoveDirection.Left, false },
                { MoveDirection.Right, false }
            };
        }

        public virtual void UpdateMovement()
        {
            throw new System.NotImplementedException();
        }

        public virtual void EnableMovement()
        {
            mover.IsMovementEnabled = true;
        }

        public virtual void DisableMovement()
        {
            mover.IsMovementEnabled = false;
        }
    }
}