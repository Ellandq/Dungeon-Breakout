using System.Collections.Generic;
using System.Linq;
using UnityEngine;
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
        private int _originX, _originY;
        
        protected override void Awake()
        {
            base.Awake();
            var bounds = tileMap.cellBounds;
            _width = bounds.size.x;
            _height = bounds.size.y;
            _walkableGrid = new bool[_width, _height];
            _originX = tileMap.cellBounds.x;
            _originY = tileMap.cellBounds.y;

            for (var x = 0; x < _width; x++)
            {
                for (var y = 0; y < _height; y++)
                {
                    var tilePos = new Vector3Int(bounds.x + x, bounds.y + y, 0);
                    var tile = tileMap.GetTile(tilePos);
                    
                    _walkableGrid[x, y] = IsTileClear(x, y);
                }
            }
        }
        
        public List<Vector3> FindPath(Vector3Int start, Vector3Int target)
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

            var startX = start.x - _originX;
            var startY = start.y - _originY;
            var targetX = target.x - _originX;
            var targetY = target.y - _originY;
            
            if (!IsValidGridIndex(startX, startY) || !IsValidGridIndex(targetX, targetY))
                return null;

            var startNode = nodeGrid[startX, startY];
            var targetNode = nodeGrid[targetX, targetY];


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
                    var cellPath = RetracePath(startNode, targetNode);
                    return ConvertPathToWorld(cellPath);
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
        
        private List<Vector3> ConvertPathToWorld(List<Vector3Int> cellPath)
        {
            return cellPath
                .Select(cell =>
                {
                    var world = tileMap.GetCellCenterWorld(new Vector3Int(cell.x + _originX, cell.y + _originY, 0));
                    return new Vector3(Mathf.Floor(world.x), Mathf.Floor(world.y), 0);
                })
                .ToList();
        }

        
        private bool IsTileClear(int tx, int ty)
        {
            var offsets = new int[,]
            {
                { 0, 0 },
                { -1, 0 },
                { 0, -1 },
                { -1, -1 }
            };

            var bounds = tileMap.cellBounds;

            for (var i = 0; i < offsets.GetLength(0); i++)
            {
                var x = tx + offsets[i, 0];
                var y = ty + offsets[i, 1];
                
                if (x < 0 || y < 0 || x >= _width || y >= _height)
                    return false;

                var tile = tileMap.GetTile(new Vector3Int(bounds.x + x, bounds.y + y, 0));
                if (tile != null && obstacleTiles.Contains(tile))
                    return false;
            }

            return true;
        }

        
        private bool IsValidGridIndex(int x, int y)
        {
            return x >= 0 && x < _width && y >= 0 && y < _height;
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