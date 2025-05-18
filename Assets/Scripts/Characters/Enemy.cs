using Characters.Movement;
using UnityEngine;

namespace Characters
{
    public class Enemy
    {
        [Header("Object References")]
        [SerializeField] private CustomCharacterController controller;

        private void Start()
        {
            controller.Initialize();
        }

        private void Update()
        {
            controller.UpdateMovement();
        }
    }
}