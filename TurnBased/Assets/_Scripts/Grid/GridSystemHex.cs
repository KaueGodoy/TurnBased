using System;
using System.Collections.Generic;
using UnityEngine;

public class GridSystemHex<TGridObject>
{
    private int _width;
    private int _height;
    private float _cellSize;
    private TGridObject[,] _gridObjectArray;

    private const float HexVerticalOffsetMultiplier = 0.75f;

    public GridSystemHex(int width, int height, float cellSize, Func<GridSystemHex<TGridObject>, GridPosition, TGridObject> createGridObject)
    {
        _width = width;
        _height = height;
        _cellSize = cellSize;

        _gridObjectArray = new TGridObject[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                _gridObjectArray[x, z] = createGridObject(this, gridPosition);
                //Debug.DrawLine(GetWorldPosition(x, z), GetWorldPosition(x, z) + Vector3.right * .2f, Color.white, 1000);
            }
        }
    }

    public Vector3 GetWorldPosition(GridPosition gridPosition)
    {
        return
            new Vector3(gridPosition.x, 0, 0) * _cellSize +
            new Vector3(0, 0, gridPosition.z) * _cellSize * HexVerticalOffsetMultiplier +
            (((gridPosition.z % 2) == 1) ? new Vector3(1, 0, 0) * _cellSize * .5f : Vector3.zero);
    }

    public GridPosition GetGridPosition(Vector3 worldPosition)
    {
        GridPosition roughXZ = new GridPosition(
            Mathf.RoundToInt(worldPosition.x / _cellSize),
            Mathf.RoundToInt(worldPosition.z / _cellSize / HexVerticalOffsetMultiplier)
        );

        bool oddRow = roughXZ.z % 2 == 1;

        List<GridPosition> neighbourGridPositionList = new List<GridPosition>
        {
            roughXZ + new GridPosition(-1, 0),
            roughXZ + new GridPosition(+1, 0),

            roughXZ + new GridPosition(0, +1),
            roughXZ + new GridPosition(0, -1),

            roughXZ + new GridPosition(oddRow ? +1 : -1, +1),
            roughXZ + new GridPosition(oddRow ? +1 : -1, -1),
        };

        GridPosition closestGridPosition = roughXZ;

        foreach (GridPosition neighbourGridPosition in neighbourGridPositionList)
        {
            if (Vector3.Distance(worldPosition, GetWorldPosition(neighbourGridPosition)) <
               Vector3.Distance(worldPosition, GetWorldPosition(closestGridPosition)))
            {
                closestGridPosition = neighbourGridPosition;
            }
        }

        return closestGridPosition;
    }

    public void CreateDebugObjects(Transform debugPrefab)
    {
        for (int x = 0; x < _width; x++)
        {
            for (int z = 0; z < _height; z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                Transform debugTransform = GameObject.Instantiate(debugPrefab, GetWorldPosition(gridPosition), Quaternion.identity);
                GridDebugObject gridDebugObject = debugTransform.GetComponent<GridDebugObject>();
                gridDebugObject.SetGridObject(GetGridObject(gridPosition));
                //gridDebugObject.SetText(gridPosition.ToString());
            }
        }
    }

    public TGridObject GetGridObject(GridPosition gridPosition)
    {
        return _gridObjectArray[gridPosition.x, gridPosition.z];
    }

    public bool IsValidGridPosition(GridPosition gridPosition)
    {
        return gridPosition.x >= 0 &&
               gridPosition.z >= 0 &&
               gridPosition.x < _width &&
               gridPosition.z < _height;
    }

    public int GetWidth()
    {
        return _width;
    }

    public int GetHeight()
    {
        return _height;
    }
}
