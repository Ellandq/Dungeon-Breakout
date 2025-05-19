using Characters.Pathfinding;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomPropertyDrawer(typeof(PatrolPath))]
    public class PatrolPathDrawer : PropertyDrawer
    {
        private const float Vector3Height = 15f;
        private const float FloatHeight = 18f;
        private const float Spacing = 4f;
        private const float ButtonWidth = 60f;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            var patrolPoints = property.FindPropertyRelative("patrolPoints");
            var patrolTimes = property.FindPropertyRelative("patrolPointsWaitingTime");

            SyncListSizes(patrolPoints, patrolTimes);

            property.isExpanded = EditorGUI.Foldout(
                new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight),
                property.isExpanded,
                label);

            if (property.isExpanded)
            {
                EditorGUI.indentLevel++;
                var y = position.y + EditorGUIUtility.singleLineHeight + Spacing;

                for (var i = 0; i < patrolPoints.arraySize; i++)
                {
                    var posRect = new Rect(position.x, y, position.width - ButtonWidth - 10, Vector3Height);
                    EditorGUI.PropertyField(posRect, patrolPoints.GetArrayElementAtIndex(i), new GUIContent($"Point {i}"));

                    var removeButtonRect = new Rect(position.x + position.width - ButtonWidth, y, ButtonWidth, Vector3Height);
                    if (GUI.Button(removeButtonRect, "Remove"))
                    {
                        patrolPoints.DeleteArrayElementAtIndex(i);
                        patrolTimes.DeleteArrayElementAtIndex(i);
                        break;
                    }

                    y += Vector3Height + Spacing;

                    var waitRect = new Rect(position.x + 15, y, position.width - 15, FloatHeight);
                    EditorGUI.PropertyField(waitRect, patrolTimes.GetArrayElementAtIndex(i), new GUIContent("Waiting Time"));

                    y += FloatHeight + (Spacing * 2);
                }
                
                var addButtonRect = new Rect(position.x, y, 100, 20);
                if (GUI.Button(addButtonRect, "Add Patrol Info"))
                {
                    var go = GetGameObjectFromProperty(property);
                    if (go && go.transform.parent)
                    {
                        var parentPos = go.transform.parent.position;
                        var floored = new Vector3(Mathf.Floor(parentPos.x), Mathf.Floor(parentPos.y), Mathf.Floor(parentPos.z));

                        patrolPoints.InsertArrayElementAtIndex(patrolPoints.arraySize);
                        patrolPoints.GetArrayElementAtIndex(patrolPoints.arraySize - 1).vector3Value = floored;

                        patrolTimes.InsertArrayElementAtIndex(patrolTimes.arraySize);
                        patrolTimes.GetArrayElementAtIndex(patrolTimes.arraySize - 1).floatValue = 0f;
                    }
                }
                
                var returnButtonRect = new Rect(position.x + 110, y, 120, 20);
                if (GUI.Button(returnButtonRect, "Return To Origin"))
                {
                    var go = GetGameObjectFromProperty(property);
                    if (go && go.transform.parent && patrolPoints.arraySize > 0)
                    {
                        var origin = patrolPoints.GetArrayElementAtIndex(0).vector3Value;
                        go.transform.parent.position = origin;
                    }
                }

                EditorGUI.indentLevel--;
            }

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var patrolPoints = property.FindPropertyRelative("patrolPoints");
            if (!property.isExpanded)
                return EditorGUIUtility.singleLineHeight;

            var count = patrolPoints.arraySize;

            var height = EditorGUIUtility.singleLineHeight + Spacing;
            height += count * (Vector3Height + Spacing + FloatHeight + Spacing * 2);
            height += 24;

            return height;
        }

        private void SyncListSizes(SerializedProperty listA, SerializedProperty listB)
        {
            while (listA.arraySize < listB.arraySize)
                listA.InsertArrayElementAtIndex(listA.arraySize);

            while (listB.arraySize < listA.arraySize)
                listB.InsertArrayElementAtIndex(listB.arraySize);
        }

        private GameObject GetGameObjectFromProperty(SerializedProperty property)
        {
            var target = property.serializedObject.targetObject;

            if (target is MonoBehaviour monoBehaviour)
                return monoBehaviour.gameObject;

            return null;
        }

    }
}
