using System;
using Characters;
using UI;
using UnityEngine;

namespace Camera
{
    public class CameraFollow : MonoBehaviour
    {
        [Header("Object References")] 
        [SerializeField] private Player player;

        [Header("Movement Settings")] 
        [SerializeField] private float speed = 5f;

        private bool _isInitialized;

        public void Initialize(Player player)
        {
            this.player = player;
            var playerPos = player.transform.position;
            playerPos.z = transform.position.z;
            transform.position = playerPos;
            _isInitialized = true;
        }
        
        private void LateUpdate()
        {
            if (!_isInitialized) return;
            var playerPos = player.transform.position;
            playerPos.z = transform.position.z;
            var newPos = Vector3.Lerp(transform.position, playerPos, speed * Time.deltaTime);
            transform.position = newPos;
        }
    }
}