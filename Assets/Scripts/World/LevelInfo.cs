using System;
using System.Collections.Generic;
using Camera;
using Characters;
using UnityEngine;

namespace World
{
    
    [Serializable]
    public class EnemyInfo
    {
        public Enemy enemy;
        public Vector3 enemySpawn;
    }

    [Serializable]
    public class CameraInfo
    {
        public EnemyCamera camera;
        public float startingRotation;
    }
    
    public class LevelInfo : MonoBehaviour
    {
        [Header("Section References")] 
        [SerializeField] private Transform mapParent;
        [SerializeField] private Transform characterParent;
        [SerializeField] private Transform enemiesParent;
        [SerializeField] private Transform camerasParent;
        [SerializeField] private Transform playerTransform;
        
        [Header("Player Info")]
        [SerializeField] private Player player;
        [SerializeField] private Vector3 playerSpawn;

        [Header("Enemy Info")] 
        [SerializeField] private List<EnemyInfo> enemiesInfo;
        [SerializeField] private List<CameraInfo> camerasInfo;

        public Player Player => player;
        public Vector3 PlayerSpawn => playerSpawn;
        public List<EnemyInfo> EnemiesInfo => enemiesInfo;
        public List<CameraInfo> CamerasInfo => camerasInfo;
        
        public void DeinitializeAllCharacters()
        {
            player.Deinitialize();
            
            foreach (var pair in enemiesInfo)
            {
                pair.enemy.Deinitialize();
            }
            
            foreach (var pair in camerasInfo)
            {
                pair.camera.Deinitialize();
            }
        }
        
        public void ResetLevel()
        {
            player.Initialize(playerSpawn);
            
            foreach (var pair in enemiesInfo)
            {
                pair.enemy.Initialize(pair.enemySpawn);
            }
            
            foreach (var pair in camerasInfo)
            {
                pair.camera.Initialize(Vector3.zero, pair.startingRotation);
            }
        }

        public void UpdateInfo()
        {
            if (playerTransform)
            {
                player = playerTransform.GetComponent<Player>();
                playerSpawn = playerTransform.position;
            }
            else
            {
                Debug.LogWarning("Player Transform is not assigned.");
                player = null;
                playerSpawn = Vector3.zero;
            }

            // Clear old data
            enemiesInfo = new List<EnemyInfo>();
            camerasInfo = new List<CameraInfo>();

            if (enemiesParent)
            {
                foreach (Transform child in enemiesParent)
                {
                    var enemy = child.GetComponent<Enemy>();
                    if (enemy)
                    {
                        enemiesInfo.Add(new EnemyInfo
                        {
                            enemy = enemy,
                            enemySpawn = child.position
                        });
                    }
                }
            }
            else
            {
                Debug.LogWarning("Enemies Parent is not assigned.");
            }
            
            if (camerasParent)
            {
                foreach (Transform child in camerasParent)
                {
                    var camera = child.GetComponent<EnemyCamera>();
                    if (camera)
                    {
                        camerasInfo.Add(new CameraInfo
                        {
                            camera = camera,
                            startingRotation = child.eulerAngles.y
                        });
                    }
                }
            }
            else
            {
                Debug.LogWarning("Camera Parent is not assigned.");
            }
        }

        public Player GetPlayer()
        {
            return player;
        }
    }
}