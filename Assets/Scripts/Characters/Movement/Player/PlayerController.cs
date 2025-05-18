using System;
using Input;

namespace Characters.Movement.Player
{
    public class PlayerController : CustomCharacterController
    {
        public override void Initialize()
        {
            mover.IsMovementEnabled = true;
        }

        public override void UpdateMovement()
        {
            
        }
        
        private void SubscribeToInputEvents()
        {
            var inputHandle = InputManager.getKeyboardInputHandle();
            
            inputHandle.AddListenerOnInputAction(state => ChangeMoveDirection(MoveDirection.Up, state), "Move Forwards");
            inputHandle.AddListenerOnInputAction(state => ChangeMoveDirection(MoveDirection.Down, state), "Move Backwards");
            inputHandle.AddListenerOnInputAction(state => ChangeMoveDirection(MoveDirection.Left, state), "Move Left");
            inputHandle.AddListenerOnInputAction(state => ChangeMoveDirection(MoveDirection.Right, state), "Move Right");
            
            // TODO add input for sprinting and sneaking
        }
        
        private void ChangeMoveDirection(MoveDirection direction, ButtonState buttonState)
        {
            MoveDirections[direction] = buttonState == ButtonState.Down;
        }
    }
}