using UnityEngine;

namespace Characters.Movement
{
    public class CharacterMover : MonoBehaviour
    {
        [Header("Character Components")]
        [SerializeField] private Rigidbody2D rigidbody;
        
        private const float GlobalSpeedMultiplier = 500f;
        
        public bool IsMovementEnabled { get; set; }
        
        public void Move(Vector2 input)
        {
            if (!IsMovementEnabled) return;
            input *= Time.deltaTime * GlobalSpeedMultiplier;
            rigidbody.AddForce(input, ForceMode2D.Impulse);
        }
    }
}
