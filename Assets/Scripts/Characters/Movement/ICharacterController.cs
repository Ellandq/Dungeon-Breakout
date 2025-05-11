namespace Characters.Movement
{
    public interface ICharacterController
    {
        void Initialize();
        void UpdateMovement();
        void Crouch();
        void EnableMovement();
        void DisableMovement();
    }
}