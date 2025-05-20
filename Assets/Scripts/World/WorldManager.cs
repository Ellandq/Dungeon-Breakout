using System;
using System.Collections.Generic;
using Camera;
using GameStates;
using UnityEngine;

namespace World
{
    public class WorldManager : ManagerBase<WorldManager>
    {
        [Header("Object References")]
        [SerializeField] private GameObject currentLevel;
        [SerializeField] private LevelInfo currentLevelInfo;
        [SerializeField] private Transform worldTransform;
        [SerializeField] private CameraFollow cameraFollow;
        
        [Header("Maps")] 
        [SerializeField] private List<GameObject> mapPrefabs;

        [Header("Events")]
        private Action _onCameraAlert;

        public void LoadLevel(int levelIndex)
        {
            if (levelIndex >= mapPrefabs.Count)
            {
                Debug.LogWarning($"There is no level with index: ${levelIndex}");
                return;
            }
            if (currentLevel) Destroy(currentLevel);
            currentLevel = Instantiate(mapPrefabs[levelIndex], worldTransform);
            currentLevelInfo = currentLevel.GetComponent<LevelInfo>();
            cameraFollow.Initialize(currentLevelInfo.GetPlayer());
            
            currentLevelInfo.ResetLevel();
            
            GameManager.ChangeState(new PlayState());
        }

        public void StopAllActors()
        {
            currentLevelInfo.DeinitializeAllCharacters();
        }

        public void RestartCurrentLevel()
        {
            _onCameraAlert = null;
            currentLevelInfo.ResetLevel();
        }

        public void AlertAllEnemies()
        {
            _onCameraAlert?.Invoke();
        }

        public void SubscribeToOnCameraAlert(Action actionToAdd)
        {
            _onCameraAlert += actionToAdd;
        }
    }
}