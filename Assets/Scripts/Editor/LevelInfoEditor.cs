using UnityEditor;
using UnityEngine;
using World;

namespace Editor
{
    [CustomEditor(typeof(LevelInfo))]
    public class LevelInfoEditor : UnityEditor.Editor
    {
        private SerializedProperty _mapParent;
        private SerializedProperty _charactersParent;
        private SerializedProperty _enemiesParent;
        private SerializedProperty _camerasParent;
        private SerializedProperty _playerTransform;

        private void OnEnable()
        {
            _mapParent = serializedObject.FindProperty("mapParent");
            _charactersParent = serializedObject.FindProperty("characterParent");
            _enemiesParent = serializedObject.FindProperty("enemiesParent");
            _camerasParent = serializedObject.FindProperty("camerasParent");
            _playerTransform = serializedObject.FindProperty("playerTransform");
        }

        public override void OnInspectorGUI()
        {
            LevelInfo levelInfo = (LevelInfo)target;
            serializedObject.Update();

            // Editable: Section References
            EditorGUILayout.LabelField("Section References", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(_mapParent);
            EditorGUILayout.PropertyField(_charactersParent);
            EditorGUILayout.PropertyField(_enemiesParent);
            EditorGUILayout.PropertyField(_camerasParent);
            EditorGUILayout.PropertyField(_playerTransform);

            EditorGUILayout.Space();

            // Readonly: Player Info
            GUI.enabled = false;
            EditorGUILayout.LabelField("Player Info", EditorStyles.boldLabel);
            EditorGUILayout.ObjectField("Player", levelInfo.Player, typeof(Object), true);
            EditorGUILayout.Vector3Field("Spawn Position", levelInfo.PlayerSpawn);

            EditorGUILayout.Space();

            // Readonly: Enemy Info
            EditorGUILayout.LabelField("Enemy Info", EditorStyles.boldLabel);
            var enemies = levelInfo.EnemiesInfo;
            if (enemies != null)
            {
                for (int i = 0; i < enemies.Count; i++)
                {
                    EditorGUILayout.LabelField($"Enemy {i + 1}", EditorStyles.miniBoldLabel);
                    EditorGUILayout.ObjectField("Enemy", enemies[i].enemy, typeof(Object), true);
                    EditorGUILayout.Vector3Field("Spawn Position", enemies[i].enemySpawn);
                }
            }

            EditorGUILayout.Space();

            // Readonly: Camera Info
            EditorGUILayout.LabelField("Camera Info", EditorStyles.boldLabel);
            var cameras = levelInfo.CamerasInfo;
            if (cameras != null)
            {
                for (int i = 0; i < cameras.Count; i++)
                {
                    EditorGUILayout.LabelField($"Camera {i + 1}", EditorStyles.miniBoldLabel);
                    EditorGUILayout.ObjectField("Camera", cameras[i].camera, typeof(Object), true);
                    EditorGUILayout.FloatField("Starting Rotation", cameras[i].startingRotation);
                }
            }

            GUI.enabled = true;

            EditorGUILayout.Space();
            if (GUILayout.Button("Update Info"))
            {
                levelInfo.UpdateInfo();
                EditorUtility.SetDirty(levelInfo);
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
