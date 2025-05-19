using System;
using Characters.Movement;
using GameStates;
using UnityEngine;

namespace Characters
{
    public class Player : MonoBehaviour
    {
        [Header("Object References")]
        [SerializeField] private CustomCharacterController controller;
        [SerializeField] private CircleCollider2D trigger;
        
        [Header("Settings")]
        [SerializeField] private LayerMask layerMask;

        private void Start()
        {
            controller.Initialize();
        }

        private void Update()
        {
            controller.UpdateMovement();
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            var objLayer = other.gameObject.layer;
            if (((1 << objLayer) & layerMask) == 0) return;
            if (objLayer == LayerMask.NameToLayer("Enemy"))
            {
                controller.DisableMovement();
                GameManager.Instance.ChangeState(new GameOverState());
            }
            else
            {
                    
            }
        }
    }
}