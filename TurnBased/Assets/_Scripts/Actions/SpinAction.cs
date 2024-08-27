using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class SpinAction : BaseAction
{
    private float _totalSpinAmount;

    protected override void Awake()
    {
        base.Awake();
        ActionName = "Spin";
    }

    private void Update()
    {
        if (!_isActive) return;

        float spinAddAmount = 360f * Time.deltaTime;
        transform.eulerAngles += new Vector3(0, spinAddAmount, 0);

        _totalSpinAmount += spinAddAmount;
        if (_totalSpinAmount >= 360f)
        {
            _isActive = false;
            _onActionComplete();
        }
    }

    public override void TakeAction(GridPosition griPosition, Action _onActionComplete)
    {
        this._onActionComplete = _onActionComplete;
        _isActive = true;
        _totalSpinAmount = 0f;
        Debug.Log("Spin activated");
    }

    public override string GetActionName()
    {
        return ActionName;
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        GridPosition unitGridPosition = _unit.GetGridPosition();

        return new List<GridPosition>
        {
            unitGridPosition
        };
    }

    public override int GetActionPointsCost()
    {
        return 2;
    }
}
