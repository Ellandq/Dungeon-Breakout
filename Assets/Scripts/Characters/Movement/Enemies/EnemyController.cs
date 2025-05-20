using System;
using System.Collections;
using Characters.Pathfinding;
using System.Collections.Generic;
using UnityEngine;
using World;
using Random = UnityEngine.Random;

namespace Characters.Movement.Enemies
{
    public class EnemyController : CustomCharacterController
    {
        [Header("Object References")]
        [SerializeField] private VisionCone visionCone;
        [SerializeField] private global::Characters.Player player;
        
        [Header("Enemy Info")] 
        [SerializeField] private EnemyState currentState;
        [SerializeField] private float originRotation;
        private bool _canSeePlayer;
        
        [Header("Movement Info")] 
        [SerializeField] private float reachThreshold = 0.1f;
        [SerializeField] private PatrolPath patrolPath;
        [SerializeField] private float virtualRotation;
        [SerializeField] private float rotationSpeed = 5f;
        private List<Vector3> _currentPath;
        private int _pathIndex;
        private bool _isRefreshingPath;
        private bool _fallbackToDirectMovement;
        private bool _freshInitialization;
        
        public override void Initialize()
        {
            WorldManager.Instance.SubscribeToOnCameraAlert((() => ChangeState(EnemyState.Searching)));
            _freshInitialization = true;
            base.Initialize();
            patrolPath.ResetPath();

            _currentPath = null;
            _pathIndex = 0;
            _isRefreshingPath = false;
            _canSeePlayer = false;

            visionCone.Initialize(PlayerFound, PlayerLost);
            ChangeState(EnemyState.Patrolling);
        }


        public override void UpdateMovement()
        {
            if (currentState is EnemyState.Stationary or EnemyState.LookingAround) return;

            if (_fallbackToDirectMovement && currentState == EnemyState.Chase)
            {
                var direction = (player.transform.position - transform.position).normalized;
                SmoothRotateTowards(direction);
                mover.Move(direction * movementSettings.GetSpeed(movementType));
                return;
            }

            if (_currentPath == null || _currentPath.Count == 0)
            {
                if (currentState == EnemyState.Chase) RefreshPathToPlayer();
                if (currentState == EnemyState.Searching && !_freshInitialization) SetNewPathToPlayer();
                return;
            }

            var target = _currentPath[_pathIndex];
            var moveDir = (target - transform.position).normalized;

            SmoothRotateTowards(moveDir);
            mover.Move(moveDir * movementSettings.GetSpeed(movementType));
            CheckNextMove();
        }


        private void CheckNextMove()
        {
            var target = _currentPath[_pathIndex];
            if (!(Vector3.Distance(transform.position, target) < reachThreshold)) return;
            
            _pathIndex++;
            if (_pathIndex < _currentPath.Count) return;
            
            switch (currentState)
            {
                case EnemyState.Patrolling:
                    var waitTime = patrolPath.GetWaitTime();
                    if (waitTime != 0f) ChangeState(EnemyState.LookingAround);
                    else
                    {
                        patrolPath.MoveNext();
                        SetNewPath();
                    }
                    break;
                case EnemyState.Chase:
                    StopAllCoroutines();
                    RefreshPathToPlayer();
                    break;
                case EnemyState.Searching:
                    ChangeState(EnemyState.LookingAround);
                    break;
            }
        }

        private void LateUpdate()
        {
            visionCone.UpdateVisionCone(virtualRotation);
        }

        private void ChangeState(EnemyState newState)
        {
            do
            {
                currentState = newState;
                _freshInitialization = false;
                visionCone.ChangeMaterial(currentState);
                switch (currentState)
                {
                    case EnemyState.Stationary:
                        virtualRotation = originRotation;
                        mover.IsMovementEnabled = false;
                        break;
                    
                    case EnemyState.Patrolling:
                        if (patrolPath.HasPoints())
                        {
                            patrolPath.MoveNext();
                            movementType = MovementType.Walking;
                            mover.IsMovementEnabled = true;
                            SetNewPath();
                        }
                        else
                        {
                            newState = EnemyState.Stationary;
                            continue;
                        }
                        break;
                    
                    case EnemyState.Chase:
                        movementType = MovementType.Sprinting;
                        mover.IsMovementEnabled = true;
                        RefreshPathToPlayer();
                        break;

                    
                    case EnemyState.Searching:
                        movementType = MovementType.Sprinting;
                        mover.IsMovementEnabled = true;
                        StopAllCoroutines();
                        SetNewPathToPlayer();
                        break;
                    
                    case EnemyState.LookingAround:
                        mover.IsMovementEnabled = false;
                        StartCoroutine(LookAroundAndResumePatrol(patrolPath.GetWaitTime()));
                        break;
                    
                    default:
                        return;
                }
                break;
            } while (true);
        }

        #region Path Setting

        private void SetNewPath()
        {
            var startCell = new Vector3Int(Mathf.FloorToInt(transform.position.x), Mathf.FloorToInt(transform.position.y), 0);
            var targetCell = patrolPath.GetCurrentConvertedPosition();
            
            SetPath(startCell, targetCell);
        }
        
        private void SetNewPathToPlayer()
        {
            var playerPos = player.transform.position;
            var startCell = new Vector3Int(Mathf.FloorToInt(transform.position.x), Mathf.FloorToInt(transform.position.y), 0);
            var targetCell = new Vector3Int(Mathf.FloorToInt(playerPos.x), Mathf.FloorToInt(playerPos.y), 0);

            SetPath(startCell, targetCell);
        }

        private void SetPath(Vector3Int startCell, Vector3Int targetCell)
        {
            _currentPath = PathfindingManager.Instance.FindPath(startCell, targetCell);
    
            if (_currentPath is { Count: > 0 })
            {
                _pathIndex = 0;
                _fallbackToDirectMovement = false;
            }
            else
            {
                _currentPath = null;
                if (currentState == EnemyState.Chase && _canSeePlayer)
                {
                    _fallbackToDirectMovement = true;
                }
            }
        }


        #endregion

        #region Player Detection

        private void PlayerFound()
        {
            _canSeePlayer = true;
            StopAllCoroutines();
            ChangeState(EnemyState.Chase);
        }

        private void PlayerLost()
        {
            if (!_canSeePlayer) return;
            _canSeePlayer = false;
            ChangeState(EnemyState.Searching);
        }

        #endregion

        #region Utils

        private void SmoothRotateTowards(Vector3 direction)
        {
            if (direction == Vector3.zero)
                return;

            var currentRot = Quaternion.Euler(0, 0, virtualRotation);

            var targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            var targetRot = Quaternion.Euler(0, 0, targetAngle);

            var smoothRot = Quaternion.Lerp(currentRot, targetRot, rotationSpeed * Time.deltaTime);

            virtualRotation = smoothRot.eulerAngles.z;
        }

        private void RefreshPathToPlayer()
        {
            SetNewPathToPlayer();
            if (!_canSeePlayer || _isRefreshingPath) return;
            StartCoroutine(RefreshPathToPlayerTimer());
        }
        
        #endregion

        #region Enumerators

        private IEnumerator RefreshPathToPlayerTimer()
        {
            _isRefreshingPath = true;
            yield return new WaitForSeconds(0.2f);
            _isRefreshingPath = false;
            RefreshPathToPlayer();
        }
        
        private IEnumerator LookAroundAndResumePatrol(float lookTime = 3f)
        {
            const float rotateDuration = 0.4f;
            var elapsedTime = 0f;

            var currentAngle = virtualRotation;

            while (elapsedTime < lookTime)
            {
                var targetAngle = Random.Range(0f, 360f);
                var rotateTime = 0f;

                while (rotateTime < rotateDuration)
                {
                    rotateTime += Time.deltaTime;
                    elapsedTime += Time.deltaTime;

                    virtualRotation = Mathf.LerpAngle(currentAngle, targetAngle, rotateTime / rotateDuration);
                    visionCone.UpdateVisionCone(virtualRotation);

                    yield return null;

                    if (elapsedTime >= lookTime)
                        break;
                }

                currentAngle = targetAngle;
            }
            ChangeState(EnemyState.Patrolling);
        }

        #endregion
    }
}
