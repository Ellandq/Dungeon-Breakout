using Input;
using Utils;

namespace Characters.Movement.Player
{
    public class PlayerController : CustomCharacterController
    {
        public override void Initialize()
        {
            base.Initialize();
            SubscribeToInputEvents();
            mover.IsMovementEnabled = true;
        }

        public override void UpdateMovement()
        {
            var speed = movementSettings.GetSpeed(movementType);
            var directions = Vector2Utils.ConvertFromDirections(moveDirections);
            
            mover.Move(speed * directions);
        }
        
        private void SubscribeToInputEvents()
        {
            var inputHandle = InputManager.getKeyboardInputHandle();
            
            // Basic movement
            inputHandle.AddListenerOnInputAction(state => ChangeMoveDirection(MoveDirection.Up, state), "Move Forwards");
            inputHandle.AddListenerOnInputAction(state => ChangeMoveDirection(MoveDirection.Down, state), "Move Backwards");
            inputHandle.AddListenerOnInputAction(state => ChangeMoveDirection(MoveDirection.Left, state), "Move Left");
            inputHandle.AddListenerOnInputAction(state => ChangeMoveDirection(MoveDirection.Right, state), "Move Right");
            
            // Sneaking and Sprinting
            inputHandle.AddListenerOnInputAction(state => ChangeMovementType(MovementType.Sneaking, state), "Crouch");
            inputHandle.AddListenerOnInputAction(state => ChangeMovementType(MovementType.Sprinting, state), "Sprint");
        }
        
        private void ChangeMoveDirection(MoveDirection direction, ButtonState buttonState)
        {
            moveDirections[direction] = buttonState == ButtonState.Down;
        }

        private void ChangeMovementType(MovementType type, ButtonState buttonState)
        {
            movementType = buttonState == ButtonState.Up ? MovementType.Walking : type;
        }
    }
}