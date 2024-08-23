using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAction : MonoBehaviour
{
    protected Unit _unit;
    protected bool _isActive;
    protected Action _onActionComplete;

    protected string _actionName = "Default name";
    public string ActionName { get { return _actionName; } set { _actionName = value; } }
    public abstract string GetActionName();
    public abstract void TakeAction(GridPosition gridPosition, Action onActionComplete);

    protected virtual void Awake()
    {
        _unit = GetComponent<Unit>();
    }
    public virtual bool IsValidActionGridPosition(GridPosition gridPosition)
    {
        List<GridPosition> validGridPositionList = GetValidActionGridPositionList();
        return validGridPositionList.Contains(gridPosition);
    }

    public abstract List<GridPosition> GetValidActionGridPositionList();

}
