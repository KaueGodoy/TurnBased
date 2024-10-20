using System;
using System.Collections.Generic;
using UnityEngine;

public class SwordAction : BaseAction
{
    [SerializeField] private int _maxSwordDistance = 1;
    [SerializeField] private int _damage = 100;
    public int MaxSwordDistance { get { return _maxSwordDistance; } set { _maxSwordDistance = value; } }
    public int Damage { get { return _damage; } set { _damage = value; } }

    public event EventHandler OnSwordActionStarted;
    public event EventHandler OnSwordActionCompleted;
    public static event EventHandler OnAnySwordHit;

    private enum State
    {
        SwingingSwordBeforeHit,
        SwingingSwordAfterHit,
    }

    private State _currentState;
    private float _stateTimer;
    private float _afterHitStateTimer = 0.5f;
    private float _beforeHitStateTimer = 0.7f;
    private Unit _targetUnit;

    protected override void Awake()
    {
        base.Awake();
        ActionName = "Sword";
    }

    private void Update()
    {
        if (!_isActive) return;

        _stateTimer -= Time.deltaTime;

        switch (_currentState)
        {
            case State.SwingingSwordBeforeHit:
                Vector3 aimingDirection = (_targetUnit.GetWorldPosition() - _unit.GetWorldPosition()).normalized;
                float rotateSpeed = 10f;
                transform.forward = Vector3.Lerp(transform.forward, aimingDirection, rotateSpeed * Time.deltaTime);
                break;
            case State.SwingingSwordAfterHit:
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
            case State.SwingingSwordBeforeHit:
                _currentState = State.SwingingSwordAfterHit;
                _stateTimer = _afterHitStateTimer;
                _targetUnit.TakeDamage(Damage);
                OnAnySwordHit?.Invoke(this, EventArgs.Empty);
                break;
            case State.SwingingSwordAfterHit:
                OnSwordActionCompleted?.Invoke(this, EventArgs.Empty);
                ActionComplete();
                break;
        }
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
            ActionValue = 200,
        };
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        GridPosition unitGridPosition = _unit.GetGridPosition();

        for (int x = -MaxSwordDistance; x <= MaxSwordDistance; x++)
        {
            for (int z = -MaxSwordDistance; z <= MaxSwordDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z, 0);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    // grid position with x and z >= 0 and within width and height
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
        _targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);

        _currentState = State.SwingingSwordBeforeHit;
        _stateTimer = _beforeHitStateTimer;

        OnSwordActionStarted?.Invoke(this, EventArgs.Empty);

        ActionStart(onActionComplete);
    }
}
