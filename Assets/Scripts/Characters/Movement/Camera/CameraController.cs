using System.Collections;
using UnityEngine;
using World;

namespace Characters.Movement.Camera
{
    public class CameraController : CustomCharacterController
    {
        [Header("Object References")] 
        [SerializeField] private VisionCone visionCone;
        [SerializeField] private Transform playerTransform;

        [Header("Camera Settings")] 
        [SerializeField] private float startingRotation;
        [SerializeField] private float maxAngle = 45f;
        [SerializeField] private float chaseTurnTime = 0.2f;
        [SerializeField] private float patrolAngularSpeed = 20f;
        private int _patrolDirection = 1;
        
        private float _angle;
        private float _angleVelocity;
        private EnemyState _currentState;
        private Coroutine _searchCoroutine;
        private Vector3 _lastKnownPlayerPos;

        public override void Initialize()
        {
            base.Initialize();
            visionCone.Initialize(PlayerFound, PlayerLost);

            _angle = startingRotation;
            _currentState = EnemyState.Patrolling;
        }

        public override void UpdateMovement()
        {
            switch (_currentState)
            {
                case EnemyState.Patrolling:
                    PatrolMovement();
                    break;
                case EnemyState.Chase:
                    ChaseMovement();
                    break;
                case EnemyState.Searching:
                    break;
            }

            visionCone.ChangeMaterial(_currentState);
            visionCone.UpdateVisionCone(_angle);
        }

        private void PatrolMovement()
        {
            _angle += patrolAngularSpeed * _patrolDirection * Time.deltaTime;

            var minAngle = startingRotation - maxAngle;
            var maxAngleVal = startingRotation + maxAngle;
            
            if (_angle > maxAngleVal)
            {
                _angle = maxAngleVal;
                _patrolDirection = -1;
            }
            else if (_angle < minAngle)
            {
                _angle = minAngle;
                _patrolDirection = 1;
            }
        }


        private void ChaseMovement()
        {
            _lastKnownPlayerPos = playerTransform.position;

            var dirToPlayer = (_lastKnownPlayerPos - transform.position).normalized;
            var targetAngle = Mathf.Atan2(dirToPlayer.y, dirToPlayer.x) * Mathf.Rad2Deg;

            var minAngle = startingRotation - maxAngle;
            var maxAngleVal = startingRotation + maxAngle;
            targetAngle = ClampAngle(targetAngle, minAngle, maxAngleVal);

            _angle = Mathf.SmoothDampAngle(_angle, targetAngle, ref _angleVelocity, chaseTurnTime);
        }


        private void PlayerFound()
        {
            if (_searchCoroutine != null)
            {
                StopCoroutine(_searchCoroutine);
                _searchCoroutine = null;
            }
            _currentState = EnemyState.Chase;
            _lastKnownPlayerPos = playerTransform.position;
            
            WorldManager.Instance.AlertAllEnemies();
        }

        private void PlayerLost()
        {
            if (_currentState == EnemyState.Chase)
            {
                _currentState = EnemyState.Searching;
                _lastKnownPlayerPos = playerTransform.position;

                if (_searchCoroutine != null)
                {
                    StopCoroutine(_searchCoroutine);
                }
                _searchCoroutine = StartCoroutine(SearchTimer());
            }
            WorldManager.Instance.AlertAllEnemies();
        }

        private IEnumerator SearchTimer()
        {
            var timer = 2.5f;
            while (timer > 0f)
            {
                timer -= Time.deltaTime;
                yield return null;
            }
            _currentState = EnemyState.Patrolling;
            _searchCoroutine = null;
        }

        private static float ClampAngle(float angle, float min, float max)
        {
            angle = NormalizeAngle(angle);
            min = NormalizeAngle(min);
            max = NormalizeAngle(max);

            var wrap = min > max;

            if (!wrap)
            {
                if (angle < min) return min;
                return angle > max ? max : angle;
            }
            else
            {
                if (!(angle > max) || !(angle < min)) return angle;
                var distToMin = Mathf.DeltaAngle(angle, min);
                var distToMax = Mathf.DeltaAngle(angle, max);
                return Mathf.Abs(distToMin) < Mathf.Abs(distToMax) ? min : max;
            }
        }

        private static float NormalizeAngle(float angle)
        {
            angle %= 360f;
            if (angle < 0) angle += 360f;
            return angle;
        }
    }
}
