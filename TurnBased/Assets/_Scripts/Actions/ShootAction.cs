using System;
using System.Collections.Generic;
using UnityEngine;

public class ShootAction : BaseAction
{
    [SerializeField] private int _maxShootRange = 4;

    [Header("Shooting Timers")]
    [SerializeField] private float _aimingStateTime = 1;
    [SerializeField] private float _shootingStateTime = .1f;
    [SerializeField] private float _coolOffStateTime = .5f;

    [SerializeField] private float _stateTimer;

    private enum State
    {
        Aiming,
        Shooting,
        Cooloff
    }

    private State _currentState;

    protected override void Awake()
    {
        base.Awake();
        ActionName = "Shoot";
    }

    private void Update()
    {
        if (!_isActive) return;

        _stateTimer -= Time.deltaTime;

        switch (_currentState)
        {
            case State.Aiming:
                break;
            case State.Shooting:
                break;
            case State.Cooloff:
                break;
        }

        if (_stateTimer <= 0f)
        {
            NextState();
        }

    }

    private void NextState()
    {
        switch (_currentState)
        {
            case State.Aiming:
                _currentState = State.Shooting;
                _stateTimer = _shootingStateTime;
                break;
            case State.Shooting:
                _currentState = State.Cooloff;
                _stateTimer = _coolOffStateTime;
                break;
            case State.Cooloff:
                _isActive = false;
                _onActionComplete();
                break;
        }

        Debug.Log(_currentState);
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        GridPosition unitGridPosition = _unit.GetGridPosition();

        for (int x = -_maxShootRange; x <= _maxShootRange; x++)
        {
            for (int z = -_maxShootRange; z <= _maxShootRange; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    // grid position with x and z >= 0 and within width and height
                    continue;
                }

                int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if (testDistance > _maxShootRange)
                {
                    continue;
                }

                if (!LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))
                {
                    // Grid position is empty, no Unit
                    continue;
                }

                Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition);

                if (targetUnit.IsEnemy == _unit.IsEnemy)
                {
                    // Both Units on the same 'team'
                    continue;
                }

                //Debug.Log(testGridPosition);
                validGridPositionList.Add(testGridPosition);
            }
        }

        return validGridPositionList;
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        this._onActionComplete = onActionComplete;
        _isActive = true;
        Debug.Log("Aiming");

        _currentState = State.Aiming;
        _stateTimer = _aimingStateTime;
    }

    public override string GetActionName()
    {
        return ActionName;
    }
}
