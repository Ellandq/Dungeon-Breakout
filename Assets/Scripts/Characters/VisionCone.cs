using System.Collections.Generic;
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

        [Header("Object References")]
        [SerializeField] private MeshFilter meshFilter;
        private Mesh _visionMesh;

        private void Awake()
        {
            _visionMesh = new Mesh
            {
                name = "Vision Mesh"
            };
            meshFilter.mesh = _visionMesh;
            var meshRenderer = GetComponent<MeshRenderer>();
            meshRenderer.sortingLayerName = "Midground";
            meshRenderer.sortingOrder = 1;
        }

        public void UpdateVisionCone(float rotation)
        {
            var startAngle = rotation - visionAngle / 2f;
            var angleStep = visionAngle / rayCount;

            var vertices = new List<Vector3> { Vector3.zero };
            var triangles = new List<int>();
            var worldPoints = new List<Vector2>();

            for (var i = 0; i <= rayCount; i++)
            {
                var angle = startAngle + angleStep * i;
                var dir = DirFromAngle(angle);
                var hit = Physics2D.Raycast(transform.position, dir, visionDistance, obstacleMask);

                var endpoint = hit ? hit.point : (Vector2)transform.position + dir * visionDistance;
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

            foreach (var target in targets)
            {
                var dirToTarget = ((Vector2)target.transform.position - (Vector2)transform.position).normalized;
                var angleToTarget = Vector2.Angle(transform.up, dirToTarget);

                if (!(angleToTarget < visionAngle / 2f)) continue;

                var dist = Vector2.Distance(transform.position, target.transform.position);
                var hit = Physics2D.Raycast(transform.position, dirToTarget, dist, obstacleMask);
                if (!hit)
                {
                    Debug.Log($"Target {target.name} is visible!");
                }
            }
        }
    }
}
