using Characters.Pathfinding;
using System.Collections.Generic;
using UnityEngine;

namespace Characters.Movement.Enemies
{
    public class EnemyController : CustomCharacterController
    {
        [Header("Patrol Info")]
        [SerializeField] private List<Vector3> patrolPoints;
        [SerializeField] private float reachThreshold = 0.1f;
        [SerializeField] private int currentPatrolIndex;
        [SerializeField] private bool reverseOnLoop;
        private bool _reverse;

        [Header("Movement Info")] 
        [SerializeField] private float virtualRotation;
        private List<Vector3> _currentPath;
        private int _pathIndex;
        
        [Header("Object References")]
        [SerializeField] private VisionCone visionCone;

        public override void Initialize()
        {
            base.Initialize();
            mover.IsMovementEnabled = true;
            currentPatrolIndex = 0;
            SetNewPath();
        }

        public override void UpdateMovement()
        {
            visionCone.UpdateVisionCone(virtualRotation);
            if (_currentPath == null) return;
            if (_currentPath.Count != 0)
            {
                var target = _currentPath[_pathIndex];
                var direction = (target - transform.position).normalized;
                mover.Move(direction * movementSettings.GetSpeed(movementType));
                if (!(Vector3.Distance(transform.position, target) < reachThreshold)) return;
            }

            _pathIndex++;

            if (_pathIndex < _currentPath.Count) return;
            AdvancePatrolIndex();
            SetNewPath();
        }

        private void AdvancePatrolIndex()
        {
            if (reverseOnLoop)
            {
                if (_reverse)
                {
                    currentPatrolIndex--;
                    if (currentPatrolIndex >= 0) return;
                    currentPatrolIndex = 1;
                    _reverse = false;
                }
                else
                {
                    currentPatrolIndex++;
                    if (currentPatrolIndex < patrolPoints.Count) return;
                    currentPatrolIndex = patrolPoints.Count - 2;
                    _reverse = true;
                }
            }
            else
            {
                currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Count;
            }
        }

        private void SetNewPath()
        {
            var startCell = new Vector3Int(Mathf.FloorToInt(transform.position.x), Mathf.FloorToInt(transform.position.y), 0);
            var targetCell = new Vector3Int(Mathf.FloorToInt(patrolPoints[currentPatrolIndex].x), Mathf.FloorToInt(patrolPoints[currentPatrolIndex].y), 0);;
            
            _currentPath = PathfindingManager.Instance.FindPath(startCell, targetCell);

            if (_currentPath != null)
            {
                _pathIndex = 0;
            }
            else
            {
                _currentPath = null;
            }
        }
    }
}
