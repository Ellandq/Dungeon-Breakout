using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

namespace Characters.Pathfinding
{
    public class PathfindingManager : ManagerBase<PathfindingManager>
    {
        [Header("Map Info")]
        [SerializeField] private Tilemap tileMap;
        [SerializeField] private TileBase[] obstacleTiles;
        private int _width, _height;
        private bool[,] _walkableGrid;
        
        private void Start()
        {
            var bounds = tileMap.cellBounds;
            _width = bounds.size.x;
            _height = bounds.size.y;
            _walkableGrid = new bool[_width, _height];

            for (var x = 0; x < _width; x++)
            {
                for (var y = 0; y < _height; y++)
                {
                    var tilePos = new Vector3Int(bounds.x + x, bounds.y + y, 0);
                    var tile = tileMap.GetTile(tilePos);
                    
                    _walkableGrid[x, y] = tile == null || !obstacleTiles.Contains(tile);
                }
            }
        }
        
        public List<Vector3Int> FindPath(Vector3Int start, Vector3Int target)
        {
            var openSet = new List<Node>();
            var closedSet = new HashSet<Node>();
            
            var nodeGrid = new Node[_width, _height];
            
            for (var x = 0; x < _width; x++)
            {
                for (var y = 0; y < _height; y++)
                {
                    nodeGrid[x, y] = new Node(x, y, _walkableGrid[x, y]);
                }
            }

            var startNode = nodeGrid[start.x, start.y];
            var targetNode = nodeGrid[target.x, target.y];

            openSet.Add(startNode);

            while (openSet.Count > 0)
            {
                var current = openSet[0];
                for (var i = 1; i < openSet.Count; i++)
                {
                    if (openSet[i].fCost < current.fCost || (openSet[i].fCost == current.fCost && openSet[i].hCost < current.hCost))
                        current = openSet[i];
                }

                openSet.Remove(current);
                closedSet.Add(current);

                if (current == targetNode)
                {
                    return RetracePath(startNode, targetNode);
                }

                foreach (var neighbor in GetNeighbors(current, nodeGrid))
                {
                    if (!neighbor.walkable || closedSet.Contains(neighbor))
                        continue;

                    var newGCost = current.gCost + GetDistance(current, neighbor);
                    if (newGCost >= neighbor.gCost && openSet.Contains(neighbor)) continue;
                    neighbor.gCost = newGCost;
                    neighbor.hCost = GetDistance(neighbor, targetNode);
                    neighbor.parent = current;

                    if (!openSet.Contains(neighbor))
                        openSet.Add(neighbor);
                }
            }

            return null;
        }

        private List<Vector3Int> RetracePath(Node startNode, Node endNode)
        {
            var path = new List<Vector3Int>();
            var current = endNode;

            while (current != startNode)
            {
                path.Add(new Vector3Int(current.x, current.y, 0));
                current = current.parent;
            }

            path.Reverse();
            return path;
        }

        private List<Node> GetNeighbors(Node node, Node[,] grid)
        {
            var neighbors = new List<Node>();

            for (var x = -1; x <= 1; x++)
            {
                for (var y = -1; y <= 1; y++)
                {
                    if (x == 0 && y == 0)
                        continue;

                    var checkX = node.x + x;
                    var checkY = node.y + y;

                    if (checkX < 0 || checkX >= _width || checkY < 0 || checkY >= _height) continue;
                    if (Mathf.Abs(x) == 1 && Mathf.Abs(y) == 1)
                    {
                        if (!_walkableGrid[node.x + x, node.y] || !_walkableGrid[node.x, node.y + y])
                            continue;
                    }

                    neighbors.Add(grid[checkX, checkY]);
                }
            }

            return neighbors;
        }


        private int GetDistance(Node a, Node b)
        {
            var dstX = Mathf.Abs(a.x - b.x);
            var dstY = Mathf.Abs(a.y - b.y);

            if (dstX > dstY)
                return 14 * dstY + 10 * (dstX - dstY);
            return 14 * dstX + 10 * (dstY - dstX);
        }
        
        public bool IsWalkable(int x, int y)
        {
            return _walkableGrid[x, y];
        }

        public static Tilemap GetTileMap()
        {
            return Instance.tileMap;
        }
    }
}