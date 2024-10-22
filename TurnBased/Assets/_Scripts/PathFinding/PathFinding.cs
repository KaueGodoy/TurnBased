using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MonoBehaviour
{
    public static PathFinding Instance { get; private set; }

    [SerializeField] private LayerMask _obstaclesLayerMask;
    [SerializeField] private LayerMask _floorLayerMask;
    [SerializeField] private Transform _gridDebugPrefab;
    [SerializeField] private Transform _pathFindingLinkContainer;

    private const int MoveStraightCost = 10;
    private const int MoveDiagonalCost = 14;

    private int _width;
    private int _height;
    private float _cellSize;
    private int _floorAmount;
    private List<GridSystemHex<PathNode>> _gridSystemList;
    private List<PathFindingLink> _pathFindingLinkList;

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

    public void Setup(int width, int height, float cellSize, int floorAmount)
    {
        this._width = width;
        this._height = height;
        this._cellSize = cellSize;
        this._floorAmount = floorAmount;

        _gridSystemList = new List<GridSystemHex<PathNode>>();

        for (int floor = 0; floor < floorAmount; floor++)
        {
            GridSystemHex<PathNode> gridSystem = new GridSystemHex<PathNode>(width, height, cellSize, floor, LevelGrid.FloorHeight,
                (GridSystemHex<PathNode> g, GridPosition gridPosition) => new PathNode(gridPosition));
            //_gridSystem.CreateDebugObjects(_gridDebugPrefab);

            _gridSystemList.Add(gridSystem);
        }

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                for (int floor = 0; floor < floorAmount; floor++)
                {
                    GridPosition gridPosition = new GridPosition(x, z, floor);
                    Vector3 worldPosition = LevelGrid.Instance.GetWorldPosition(gridPosition);
                    float raycastOffseDistance = 1f;

                    GetNode(x, z, floor).IsWalkable = false;

                    if (Physics.Raycast(
                        worldPosition + Vector3.up * raycastOffseDistance,
                        Vector3.down,
                        raycastOffseDistance * 2,
                        _floorLayerMask))
                    {
                        GetNode(x, z, floor).IsWalkable = true;
                    }

                    if (Physics.Raycast(
                        worldPosition + Vector3.down * raycastOffseDistance,
                        Vector3.up,
                        raycastOffseDistance * 2,
                        _obstaclesLayerMask))
                    {
                        GetNode(x, z, floor).IsWalkable = false;
                    }
                }
            }
        }

        _pathFindingLinkList = new List<PathFindingLink>();
        foreach (Transform pathFindingLinkTransform in _pathFindingLinkContainer)
        {
            if (pathFindingLinkTransform.TryGetComponent(out PathFindingLinkMonoBehaviour pathFindingLinkMonoBehaviour))
            {
                _pathFindingLinkList.Add(pathFindingLinkMonoBehaviour.GetPathFindingLink());
            }
        }
    }

    private List<GridPosition> GetPathFindingLinkConnectedGridPositionList(GridPosition gridPosition)
    {
        List<GridPosition> gridPositionList = new List<GridPosition>();

        foreach (PathFindingLink pathFindingLink in _pathFindingLinkList)
        {
            if (pathFindingLink.GridPositionA == gridPosition)
            {
                gridPositionList.Add(pathFindingLink.GridPositionB);
            }
            if (pathFindingLink.GridPositionB == gridPosition)
            {
                gridPositionList.Add(pathFindingLink.GridPositionA);
            }
        }

        return gridPositionList;
    }

    public List<GridPosition> FindPath(GridPosition startGridPosition, GridPosition endGridPosition, out int pathLength)
    {
        List<PathNode> openList = new List<PathNode>();
        List<PathNode> closedList = new List<PathNode>();

        PathNode startNode = GetGridSystem(startGridPosition.floor).GetGridObject(startGridPosition);
        PathNode endNode = GetGridSystem(endGridPosition.floor).GetGridObject(endGridPosition);

        openList.Add(startNode);

        for (int x = 0; x < _width; x++)
        {
            for (int z = 0; z < _height; z++)
            {
                for (int floor = 0; floor < _floorAmount; floor++)
                {
                    GridPosition gridPosition = new GridPosition(x, z, floor);
                    PathNode pathNode = GetGridSystem(floor).GetGridObject(gridPosition);

                    pathNode.GCost = int.MaxValue;
                    pathNode.HCost = 0;
                    pathNode.CalculateFCost();
                    pathNode.ResetCameFromPathNode();
                }
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
            Vector3.Distance(GetGridSystem(gridPositionA.floor).GetWorldPosition(gridPositionA), GetGridSystem(gridPositionB.floor).GetWorldPosition(gridPositionB)));

        //GridPosition gridPositionDistance = gridPositionA - gridPositionB;
        //int xDistance = Mathf.Abs(gridPositionDistance.x);
        //int zDistance = Mathf.Abs(gridPositionDistance.z);
        //int remaining = Mathf.Abs(xDistance - zDistance);

        //return MoveDiagonalCost * Mathf.Min(xDistance, zDistance) + MoveStraightCost * remaining;
    }

    private GridSystemHex<PathNode> GetGridSystem(int floor)
    {
        return _gridSystemList[floor];
    }

    private PathNode GetNode(int x, int z, int floor)
    {
        return GetGridSystem(floor).GetGridObject(new GridPosition(x, z, floor));
    }

    private List<PathNode> GetNeighbourList(PathNode currentNode)
    {
        List<PathNode> neighbourList = new List<PathNode>();

        GridPosition gridPosition = currentNode.GetGridPosition();

        if (gridPosition.x - 1 >= 0)
        {
            // Left
            neighbourList.Add(GetNode(gridPosition.x - 1, gridPosition.z + 0, gridPosition.floor));

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


        if (gridPosition.x + 1 < _width)
        {
            // Right
            neighbourList.Add(GetNode(gridPosition.x + 1, gridPosition.z + 0, gridPosition.floor));

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
            neighbourList.Add(GetNode(gridPosition.x + 0, gridPosition.z - 1, gridPosition.floor));
        }

        if (gridPosition.z + 1 < _height)
        {
            // Up
            neighbourList.Add(GetNode(gridPosition.x + 0, gridPosition.z + 1, gridPosition.floor));
        }

        bool oddRow = gridPosition.z % 2 == 1;

        if (oddRow)
        {
            if (gridPosition.x + 1 < _width)
            {
                if (gridPosition.z - 1 >= 0)
                {
                    neighbourList.Add(GetNode(gridPosition.x + 1, gridPosition.z - 1, gridPosition.floor));
                }

                if (gridPosition.z + 1 < _height)
                {
                    neighbourList.Add(GetNode(gridPosition.x + 1, gridPosition.z + 1, gridPosition.floor));
                }
            }
        }
        else
        {
            if (gridPosition.x - 1 >= 0)
            {
                if (gridPosition.z - 1 >= 0)
                {
                    neighbourList.Add(GetNode(gridPosition.x - 1, gridPosition.z - 1, gridPosition.floor));
                }
                if (gridPosition.z + 1 < _height)
                {
                    neighbourList.Add(GetNode(gridPosition.x - 1, gridPosition.z + 1, gridPosition.floor));
                }
            }
        }

        List<PathNode> totalNeighbourList = new List<PathNode>();
        totalNeighbourList.AddRange(neighbourList);

        List<GridPosition> pathFindingLinkGridPositionList = GetPathFindingLinkConnectedGridPositionList(gridPosition);

        foreach (GridPosition pathFindingLinkGridPosition in pathFindingLinkGridPositionList)
        {
            totalNeighbourList.Add(
                GetNode(pathFindingLinkGridPosition.x,
                        pathFindingLinkGridPosition.z,
                        pathFindingLinkGridPosition.floor
                        )
            );
        }

        //foreach (PathNode pathNode in neighbourList)
        //{
        //    GridPosition neighbourGridPosition = pathNode.GetGridPosition();

        //    if (neighbourGridPosition.floor - 1 >= 0)
        //    {
        //        totalNeighbourList.Add(GetNode(neighbourGridPosition.x, neighbourGridPosition.z, neighbourGridPosition.floor - 1));
        //    }
        //    if (neighbourGridPosition.floor + 1 < _floorAmount)
        //    {
        //        totalNeighbourList.Add(GetNode(neighbourGridPosition.x, neighbourGridPosition.z, neighbourGridPosition.floor + 1));
        //    }
        //}

        return totalNeighbourList;
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
        GetGridSystem(gridPosition.floor).GetGridObject(gridPosition).IsWalkable = isWalkable;
    }

    public bool IsWalkableGridPosition(GridPosition gridPosition)
    {
        return GetGridSystem(gridPosition.floor).GetGridObject(gridPosition).IsWalkable;
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
