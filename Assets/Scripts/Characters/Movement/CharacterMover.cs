using UnityEngine;

namespace Characters.Movement
{
    public class CharacterMover : MonoBehaviour
    {
        [Header("Character Components")]
        [SerializeField] private Rigidbody2D rigidbody;
        
        public bool IsMovementEnabled { get; set; }
        
        public void Move(Vector2 input)
        {
            if (!IsMovementEnabled) return;
            rigidbody.AddForce(input, ForceMode2D.Impulse);
        }
    }
}
