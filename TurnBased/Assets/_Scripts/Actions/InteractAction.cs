using System;
using System.Collections.Generic;
using UnityEngine;

public class InteractAction : BaseAction
{
    [SerializeField] private int _maxInteractDistance = 1;
    public int MaxInteractDistance { get { return _maxInteractDistance; } set { _maxInteractDistance = value; } }

    protected override void Awake()
    {
        base.Awake();
        ActionName = "Interact";
    }

    private void Update()
    {
        if (!_isActive) return;
    }

    public override string GetActionName()
    {
        return ActionName;
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        return new EnemyAIAction
        {
            GridPosition = gridPosition,
            ActionValue = 0,
        };
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        GridPosition unitGridPosition = _unit.GetGridPosition();

        for (int x = -MaxInteractDistance; x <= MaxInteractDistance; x++)
        {
            for (int z = -MaxInteractDistance; z <= MaxInteractDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    // grid position with x and z >= 0 and within width and height
                    continue;
                }

                Door door = LevelGrid.Instance.GetDoorAtGridPosition(testGridPosition);

                if (door == null)
                {
                    // No door on this position
                    continue;
                }

                validGridPositionList.Add(testGridPosition);
            }
        }

        return validGridPositionList;
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        Door door = LevelGrid.Instance.GetDoorAtGridPosition(gridPosition);
        door.Interact(OnInteractionComplete);

        ActionStart(onActionComplete);
    }

    private void OnInteractionComplete()
    {
        ActionComplete();
    }
}
