using System;
using System.Collections.Generic;
using Characters;
using InteractablePanel;
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
        [SerializeField] private Transform panelsParent;
        [SerializeField] private Transform playerTransform;
        
        [Header("Player Info")]
        [SerializeField] private Player player;
        [SerializeField] private Vector3 playerSpawn;

        [Header("Enemy Info")] 
        [SerializeField] private List<EnemyInfo> enemiesInfo;
        [SerializeField] private List<CameraInfo> camerasInfo;
        [SerializeField] private List<InteractablePanelObject> panels;

        public Player Player => player;
        public Vector3 PlayerSpawn => playerSpawn;
        public List<EnemyInfo> EnemiesInfo => enemiesInfo;
        public List<CameraInfo> CamerasInfo => camerasInfo;
        
        public void DeinitializeAllCharacters()
        {
            player.Deinitialize();
            
            foreach (var info in enemiesInfo)
            {
                info.enemy.Deinitialize();
            }
            
            foreach (var pair in camerasInfo)
            {
                pair.camera.Deinitialize();
            }
        }
        
        public void ResetLevel()
        {
            player.Initialize(playerSpawn);
            
            foreach (var info in enemiesInfo)
            {
                info.enemy.Initialize(info.enemySpawn);
            }
            
            foreach (var info in camerasInfo)
            {
                info.camera.Initialize(Vector3.zero, info.startingRotation);
            }
            
            foreach (var panel in panels)
            {
                panel.Initialize();
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
            
            if (panelsParent)
            {
                foreach (Transform child in panelsParent)
                {
                    foreach (Transform panelChild in child)
                    {
                        var panel = panelChild.GetComponent<InteractablePanelObject>();
                        if (panel)
                        {
                            panels.Add(panel);
                        }
                    }
                }
            }
            else
            {
                Debug.LogWarning("Panels Parent is not assigned.");
            }
        }

        public Player GetPlayer()
        {
            return player;
        }
    }
}