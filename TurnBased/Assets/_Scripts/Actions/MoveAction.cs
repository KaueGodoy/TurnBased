using System;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : BaseAction
{
    [SerializeField] private float _moveSpeed = 4f;
    [SerializeField] private float _rotateSpeed = 4f;
    [SerializeField] private int _maxMoveDistance = 4;

    public event EventHandler OnStartMoving;
    public event EventHandler OnStopMoving;

    public float MoveSpeed { get { return _moveSpeed; } set { _moveSpeed = value; } }
    public float RotateSpeed { get { return _rotateSpeed; } set { _rotateSpeed = value; } }

    private List<Vector3> _positionList;
    private int _currentPositionIndex;

    private float _stoppingDistance = 0.1f;

    protected override void Awake()
    {
        base.Awake();
        ActionName = "Move";
    }

    private void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        if (!_isActive) return;

        Vector3 targetPosition = _positionList[_currentPositionIndex];
        Vector3 moveDirection = (targetPosition - transform.position).normalized;

        transform.forward = Vector3.Lerp(transform.forward, moveDirection, RotateSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition) > _stoppingDistance)
        {
            transform.position += moveDirection * MoveSpeed * Time.deltaTime;
        }
        else
        {
            _currentPositionIndex++;

            if (_currentPositionIndex >= _positionList.Count)
            {
                OnStopMoving?.Invoke(this, EventArgs.Empty);
                ActionComplete();
            }
        }
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        List<GridPosition> pathGridPositionList = PathFinding.Instance.FindPath(_unit.GetGridPosition(), gridPosition);

        _currentPositionIndex = 0;
        _positionList = new List<Vector3>();

        foreach (GridPosition pathGridPosition in pathGridPositionList)
        {
            _positionList.Add(LevelGrid.Instance.GetWorldPosition(pathGridPosition));
        }

        OnStartMoving?.Invoke(this, EventArgs.Empty);

        ActionStart(onActionComplete);
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        GridPosition unitGridPosition = _unit.GetGridPosition();

        for (int x = -_maxMoveDistance; x <= _maxMoveDistance; x++)
        {
            for (int z = -_maxMoveDistance; z <= _maxMoveDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    // grid position with x and z >= 0 and within width and height
                    continue;
                }

                if (unitGridPosition == testGridPosition)
                {
                    // Same grid position where the unit is already at
                    continue;
                }

                if (LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))
                {
                    // There is a unit on this position = the grid obj list is not empty = count > 0
                    continue;
                }

                //Debug.Log(testGridPosition);
                validGridPositionList.Add(testGridPosition);
            }
        }

        return validGridPositionList;
    }

    public override string GetActionName()
    {
        return ActionName;
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        int targetCountAtGridPosition = _unit.GetAction<ShootAction>().GetTargetCountAtPosition(gridPosition);

        return new EnemyAIAction
        {
            GridPosition = gridPosition,
            ActionValue = targetCountAtGridPosition * 10,
        };
    }
}
