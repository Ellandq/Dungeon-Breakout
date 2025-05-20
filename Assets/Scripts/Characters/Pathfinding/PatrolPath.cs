using System.Collections.Generic;
using UnityEngine;

namespace Characters.Pathfinding
{
    [System.Serializable]
    public struct PatrolPath
    {
        [SerializeField] private List<Vector3> patrolPoints;
        [SerializeField] private List<float> patrolPointsWaitingTime;
        [SerializeField] private bool reverseOnFinish;

        private int _currentPatrolPoint;
        private bool _reverse;

        public float GetWaitTime()
        {
            return patrolPointsWaitingTime[_currentPatrolPoint];
        }

        public void MoveNext()
        {
            if (_reverse && _currentPatrolPoint == 0) _reverse = false;
            else if (reverseOnFinish && _currentPatrolPoint == patrolPoints.Count - 1) _reverse = true;
            _currentPatrolPoint += _reverse ? -1 : 1;
            if (!reverseOnFinish && _currentPatrolPoint == patrolPoints.Count) _currentPatrolPoint = 0;
        }

        public Vector3Int GetCurrentConvertedPosition()
        {
            return new Vector3Int(Mathf.FloorToInt(patrolPoints[_currentPatrolPoint].x), Mathf.FloorToInt(patrolPoints[_currentPatrolPoint].y), 0);
        }

        public bool HasPoints()
        {
            return patrolPoints.Count != 0;
        }

        public void ResetPath()
        {
            _currentPatrolPoint = 0;
            _reverse = false;
        }
    }
}