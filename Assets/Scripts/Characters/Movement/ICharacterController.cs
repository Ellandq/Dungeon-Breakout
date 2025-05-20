namespace Characters.Movement
{
    public interface ICharacterController
    {
        void Initialize();
        void UpdateMovement();
        void EnableMovement();
        void DisableMovement();
    }
}