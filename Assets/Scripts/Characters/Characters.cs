using Characters.Movement;
using UnityEngine;

namespace Characters
{
    public abstract class Characters : MonoBehaviour
    {
        [Header("Object References")]
        [SerializeField] protected CustomCharacterController controller;
        
        private bool _isInitialized;
        
        public virtual void Initialize(Vector3 spawnPos = new (), float rotation = 0f)
        {
            transform.position = spawnPos;
            controller.Initialize();
            _isInitialized = true;
        }
        
        public virtual void Deinitialize()
        {
            _isInitialized = false;
        }
    
        protected virtual void Update()
        {
            if (!_isInitialized) return;
            controller.UpdateMovement();
        }
    }
}