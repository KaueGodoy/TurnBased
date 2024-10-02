using System;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeAction : BaseAction
{
    protected override void Awake()
    {
        base.Awake();
        ActionName = "Grenade";
    }

    private void Update()
    {
        if (!_isActive) return;
        
        ActionComplete();
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
        GridPosition unitGridPosition = _unit.GetGridPosition();

        return new List<GridPosition>
        {
            unitGridPosition
        };
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        Debug.Log("GrenadeAction");
        ActionStart(onActionComplete);
    }
}
