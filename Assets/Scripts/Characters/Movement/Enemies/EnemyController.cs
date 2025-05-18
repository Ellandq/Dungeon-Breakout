using System;
using System.Collections.Generic;
using UnityEngine;

namespace Characters.Movement.Enemies
{
    public class EnemyController : CustomCharacterController
    {
        [Header("Patrol Info")]
        [SerializeField] private List<Vector3> patrolPoints;
        [SerializeField] private float reachThreshold = 0.1f;
        private int _currentPatrolIndex;

        [Header("Movement Info")] 
        [SerializeField] private float virtualRotation;

        public override void Initialize()
        {
            base.Initialize();
            mover.IsMovementEnabled = true;
            _currentPatrolIndex = 0;
        }

        public override void UpdateMovement()
        {
            if (patrolPoints == null || patrolPoints.Count == 0) return;

            var target = patrolPoints[_currentPatrolIndex];
            var direction = (target - transform.position).normalized;

            mover.Move(direction);

            if (Vector2.Distance(transform.position, target) < reachThreshold)
            {
                Debug.Log(_currentPatrolIndex);
                _currentPatrolIndex = (_currentPatrolIndex + 1) % patrolPoints.Count;
            }
        }
    }
}