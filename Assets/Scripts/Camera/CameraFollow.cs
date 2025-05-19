using System;
using Characters;
using UnityEngine;

namespace Camera
{
    public class CameraFollow : MonoBehaviour
    {
        [Header("Object References")] 
        [SerializeField] private Player player;

        [Header("Movement Settings")] 
        [SerializeField] private float speed = 5f;

        private void LateUpdate()
        {
            var playerPos = player.transform.position;
            playerPos.z = transform.position.z;
            var newPos = Vector3.Lerp(transform.position, playerPos, speed * Time.deltaTime);
            transform.position = newPos;
        }
    }
}