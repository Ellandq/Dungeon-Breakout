using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Characters
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class VisionCone : MonoBehaviour
    {
        [Header("Vision Cone Settings")]
        [SerializeField] private float visionDistance = 3f;
        [SerializeField] private float visionAngle = 90f;
        [SerializeField] private int rayCount = 50;
        [SerializeField] private LayerMask obstacleMask;
        [SerializeField] private LayerMask targetMask;
        private float _currentRotation;
        private Action _onPlayerFound;
        private Action _onPlayerLost;
        private bool _canSeePlayer;

        [Header("Object References")]
        [SerializeField] private MeshFilter meshFilter;
        [SerializeField] private MeshRenderer meshRenderer;
        private Mesh _visionMesh;

        [Header("Materials")] 
        [SerializeField] private Material chaseMat;
        [SerializeField] private Material alertMat;
        [SerializeField] private Material normalMat;

        public void Initialize(Action onPlayerFound, Action onPlayerLost)
        {
            _visionMesh = new Mesh
            {
                name = "Vision Mesh"
            };
            meshFilter.mesh = _visionMesh;
            meshRenderer.sortingLayerName = "Midground";
            meshRenderer.sortingOrder = 1;
            _onPlayerFound = onPlayerFound;
            _onPlayerLost = onPlayerLost;
        }

        public void UpdateVisionCone(float rotation)
        {
            if (!_visionMesh)
            {
                _visionMesh = new Mesh { name = "Vision Mesh" };
                meshFilter.mesh = _visionMesh;
            }
            
            _currentRotation = rotation;
            var startAngle = _currentRotation - visionAngle / 2f;
            var angleStep = visionAngle / rayCount;

            var vertices = new List<Vector3> { Vector3.zero };
            var triangles = new List<int>();
            var worldPoints = new List<Vector2>();

            for (var i = 0; i <= rayCount; i++)
            {
                var angle = startAngle + angleStep * i;
                var dir = DirFromAngle(angle);
                var hit = Physics2D.Raycast(transform.position, dir, visionDistance, obstacleMask);
                Vector2 endpoint;

                if (hit.collider)
                {
                    endpoint = hit.point;
                }
                else
                {
                    endpoint = (Vector2)transform.position + dir * visionDistance;
                }

                worldPoints.Add(endpoint);

                var localVertex = transform.InverseTransformPoint(endpoint);
                vertices.Add(localVertex);
            }

            for (var i = 1; i < vertices.Count - 1; i++)
            {
                triangles.Add(0);
                triangles.Add(i);
                triangles.Add(i + 1);
            }
            
            _visionMesh.Clear();
            _visionMesh.vertices = vertices.ToArray();
            _visionMesh.triangles = triangles.ToArray();
            _visionMesh.RecalculateNormals();

            DetectTargets(worldPoints);
        }

        private Vector2 DirFromAngle(float angleInDegrees)
        {
            var rad = angleInDegrees * Mathf.Deg2Rad;
            return new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
        }

        private void DetectTargets(List<Vector2> coneEdges)
        {
            var targets = Physics2D.OverlapCircleAll(transform.position, visionDistance, targetMask);
            var playerIsVisible = (from target in targets
                let dirToTarget = ((Vector2)target.transform.position - (Vector2)transform.position).normalized
                let forward = DirFromAngle(_currentRotation)
                let angleToTarget = Vector2.Angle(forward, dirToTarget)
                where angleToTarget < visionAngle / 2f
                let dist = Vector2.Distance(transform.position, target.transform.position)
                select Physics2D.Raycast(transform.position, dirToTarget, dist, obstacleMask)).Any(hit => !hit);

            switch (playerIsVisible)
            {
                case true when !_canSeePlayer:
                    _onPlayerFound?.Invoke();
                    _canSeePlayer = true;
                    break;
                case false when _canSeePlayer:
                    _onPlayerLost?.Invoke();
                    _canSeePlayer = false;
                    break;
            }
        }

        public void ChangeMaterial(EnemyState state)
        {
            switch (state)
            {
                case EnemyState.Stationary:
                case EnemyState.LookingAround:
                case EnemyState.Patrolling:
                    meshRenderer.material = normalMat;
                    break;
                case EnemyState.Chase:
                    meshRenderer.material = chaseMat;
                    break;
                case EnemyState.Searching:
                    meshRenderer.material = alertMat;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }

    }
}
