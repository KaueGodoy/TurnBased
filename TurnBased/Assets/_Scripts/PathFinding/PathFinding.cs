using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MonoBehaviour
{
    public static PathFinding Instance { get; private set; }

    [SerializeField] private LayerMask _obstaclesLayerMask;
    [SerializeField] private Transform _gridDebugPrefab;

    private const int MoveStraightCost = 10;
    private const int MoveDiagonalCost = 14;

    private int _width;
    private int _height;
    private float _cellSize;
    private GridSystemHex<PathNode> _gridSystem;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Instance already exists" + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void Setup(int width, int height, float cellSize)
    {
        this._width = width;
        this._height = height;
        this._cellSize = cellSize;

        _gridSystem = new GridSystemHex<PathNode>(width, height, cellSize, 0, LevelGrid.FloorHeight,
            (GridSystemHex<PathNode> g, GridPosition gridPosition) => new PathNode(gridPosition));
        //_gridSystem.CreateDebugObjects(_gridDebugPrefab);

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                GridPosition gridPosition = new GridPosition(x, z, 0);
                Vector3 worldPosition = LevelGrid.Instance.GetWorldPosition(gridPosition);
                float raycastOffseDistance = 5f;
                if (Physics.Raycast(
                    worldPosition + Vector3.down * raycastOffseDistance,
                    Vector3.up,
                    raycastOffseDistance * 2,
                    _obstaclesLayerMask))
                {
                    GetNode(x, z).IsWalkable = false;
                }
            }
        }
    }

    public List<GridPosition> FindPath(GridPosition startGridPosition, GridPosition endGridPosition, out int pathLength)
    {
        List<PathNode> openList = new List<PathNode>();
        List<PathNode> closedList = new List<PathNode>();

        PathNode startNode = _gridSystem.GetGridObject(startGridPosition);
        PathNode endNode = _gridSystem.GetGridObject(endGridPosition);

        openList.Add(startNode);

        for (int x = 0; x < _gridSystem.GetWidth(); x++)
        {
            for (int z = 0; z < _gridSystem.GetHeight(); z++)
            {
                GridPosition gridPosition = new GridPosition(x, z, 0);
                PathNode pathNode = _gridSystem.GetGridObject(gridPosition);

                pathNode.GCost = int.MaxValue;
                pathNode.HCost = 0;
                pathNode.CalculateFCost();
                pathNode.ResetCameFromPathNode();

            }
        }

        startNode.GCost = 0;
        startNode.HCost = CalculateHeuristicDistance(startGridPosition, endGridPosition);
        startNode.CalculateFCost();

        while (openList.Count > 0)
        {
            PathNode currentNode = GetLowestFCostPathNode(openList);

            if (currentNode == endNode)
            {
                // Reached final node
                pathLength = endNode.FCost;
                return CalculatePath(endNode);
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            foreach (PathNode neighbourNode in GetNeighbourList(currentNode))
            {
                if (closedList.Contains(neighbourNode))
                {
                    continue;
                }

                if (!neighbourNode.IsWalkable)
                {
                    closedList.Add(neighbourNode);
                    continue;
                }

                int tentativeGCost =
                    currentNode.GCost + MoveStraightCost;

                if (tentativeGCost < neighbourNode.GCost)
                {
                    neighbourNode.SetCameFromPathNode(currentNode);
                    neighbourNode.GCost = tentativeGCost;
                    neighbourNode.HCost = CalculateHeuristicDistance(neighbourNode.GetGridPosition(), endGridPosition);
                    neighbourNode.CalculateFCost();

                    if (!openList.Contains(neighbourNode))
                    {
                        openList.Add(neighbourNode);
                    }
                }


            }
        }

        // No path found
        pathLength = 0;
        return null;
    }

    private List<GridPosition> CalculatePath(PathNode endNode)
    {
        List<PathNode> pathNodeList = new List<PathNode>();
        pathNodeList.Add(endNode);

        PathNode currentNode = endNode;

        while (currentNode.GetCameFromPathNode() != null)
        {
            pathNodeList.Add(currentNode.GetCameFromPathNode());
            currentNode = currentNode.GetCameFromPathNode();
        }

        pathNodeList.Reverse();

        List<GridPosition> gridPositionList = new List<GridPosition>();
        foreach (PathNode pathNode in pathNodeList)
        {
            gridPositionList.Add(pathNode.GetGridPosition());
        }

        return gridPositionList;
    }

    public int CalculateHeuristicDistance(GridPosition gridPositionA, GridPosition gridPositionB)
    {
        return Mathf.RoundToInt(MoveStraightCost *
            Vector3.Distance(_gridSystem.GetWorldPosition(gridPositionA), _gridSystem.GetWorldPosition(gridPositionB)));

        //GridPosition gridPositionDistance = gridPositionA - gridPositionB;
        //int xDistance = Mathf.Abs(gridPositionDistance.x);
        //int zDistance = Mathf.Abs(gridPositionDistance.z);
        //int remaining = Mathf.Abs(xDistance - zDistance);

        //return MoveDiagonalCost * Mathf.Min(xDistance, zDistance) + MoveStraightCost * remaining;
    }

    private PathNode GetNode(int x, int z)
    {
        return _gridSystem.GetGridObject(new GridPosition(x, z, 0));
    }

    private List<PathNode> GetNeighbourList(PathNode currentNode)
    {
        List<PathNode> neighbourList = new List<PathNode>();

        GridPosition gridPosition = currentNode.GetGridPosition();

        if (gridPosition.x - 1 >= 0)
        {
            // Left
            neighbourList.Add(GetNode(gridPosition.x - 1, gridPosition.z + 0));

            //if (gridPosition.z - 1 >= 0)
            //{
            //    // Left Down
            //    neighbourList.Add(GetNode(gridPosition.x - 1, gridPosition.z - 1));
            //}

            //if (gridPosition.z + 1 < _gridSystem.GetHeight())
            //{
            //    // Left Up
            //    neighbourList.Add(GetNode(gridPosition.x - 1, gridPosition.z + 1));
            //}
        }


        if (gridPosition.x + 1 < _gridSystem.GetWidth())
        {
            // Right
            neighbourList.Add(GetNode(gridPosition.x + 1, gridPosition.z + 0));

            //if (gridPosition.z - 1 >= 0)
            //{
            //    // Right Down
            //    neighbourList.Add(GetNode(gridPosition.x + 1, gridPosition.z - 1));
            //}

            //if (gridPosition.z + 1 < _gridSystem.GetHeight())
            //{
            //    // Right Up
            //    neighbourList.Add(GetNode(gridPosition.x + 1, gridPosition.z + 1));
            //}
        }

        if (gridPosition.z - 1 >= 0)
        {
            // Down
            neighbourList.Add(GetNode(gridPosition.x + 0, gridPosition.z - 1));
        }

        if (gridPosition.z + 1 < _gridSystem.GetHeight())
        {
            // Up
            neighbourList.Add(GetNode(gridPosition.x + 0, gridPosition.z + 1));
        }

        bool oddRow = gridPosition.z % 2 == 1;

        if (oddRow)
        {
            if (gridPosition.x + 1 < _gridSystem.GetWidth())
            {
                if (gridPosition.z - 1 >= 0)
                {
                    neighbourList.Add(GetNode(gridPosition.x + 1, gridPosition.z - 1));
                }

                if (gridPosition.z + 1 < _gridSystem.GetHeight())
                {
                    neighbourList.Add(GetNode(gridPosition.x + 1, gridPosition.z + 1));
                }
            }
        }
        else
        {
            if (gridPosition.x - 1 >= 0)
            {
                if (gridPosition.z - 1 >= 0)
                {
                    neighbourList.Add(GetNode(gridPosition.x - 1, gridPosition.z - 1));
                }
                if (gridPosition.z + 1 < _gridSystem.GetHeight())
                {
                    neighbourList.Add(GetNode(gridPosition.x - 1, gridPosition.z + 1));
                }
            }
        }

        return neighbourList;
    }

    private PathNode GetLowestFCostPathNode(List<PathNode> pathNodeList)
    {
        PathNode lowestFCostPathNode = pathNodeList[0];

        for (int i = 0; i < pathNodeList.Count; i++)
        {
            if (pathNodeList[i].FCost < lowestFCostPathNode.FCost)
            {
                lowestFCostPathNode = pathNodeList[i];
            }
        }

        return lowestFCostPathNode;
    }

    public void SetWalkableGridPosition(GridPosition gridPosition, bool isWalkable)
    {
        _gridSystem.GetGridObject(gridPosition).IsWalkable = isWalkable;
    }

    public bool IsWalkableGridPosition(GridPosition gridPosition)
    {
        return _gridSystem.GetGridObject(gridPosition).IsWalkable;
    }

    public bool HasPath(GridPosition startGridPosition, GridPosition endGridPosition)
    {
        return FindPath(startGridPosition, endGridPosition, out int pathLength) != null;
    }

    public int GetPathLength(GridPosition startGridPosition, GridPosition endGridPosition)
    {
        FindPath(startGridPosition, endGridPosition, out int pathLength);
        return pathLength;
    }
}
