using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShootAction : BaseAction
{
    public static event EventHandler<OnShootEventArgs> OnAnyShoot;
    public event EventHandler<OnShootEventArgs> OnShoot;
    public class OnShootEventArgs : EventArgs
    {
        public Unit TargetUnit { get; set; }
        public Unit ShootingUnit { get; set; }
    }

    [SerializeField] private LayerMask _obstaclesLayerMask;
    [SerializeField] private int _maxShootRange = 4;
    public int MaxShootRange { get { return _maxShootRange; } set { _maxShootRange = value; } }

    [Header("Shooting Timers")]
    [SerializeField] private float _aimingStateTime = 1;
    [SerializeField] private float _shootingStateTime = .1f;
    [SerializeField] private float _coolOffStateTime = .5f;

    [SerializeField] private float _stateTimer;
    [Header("Damage")]
    [SerializeField] private int _damage = 60;
    public int Damage { get { return _damage; } set { _damage = value; } }

    private Unit _targetUnit;
    private bool _canShootBullet;

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
                Vector3 aimingDirection = (_targetUnit.GetWorldPosition() - _unit.GetWorldPosition()).normalized;
                aimingDirection.y = 0f;

                float rotateSpeed = 10f;
                transform.forward = Vector3.Slerp(transform.forward, aimingDirection, rotateSpeed * Time.deltaTime);
                break;
            case State.Shooting:
                if (_canShootBullet)
                {
                    Shoot();
                    _canShootBullet = false;
                }
                break;
            case State.Cooloff:
                break;
        }

        if (_stateTimer <= 0f)
        {
            NextState();
        }

    }

    private void Shoot()
    {
        OnAnyShoot?.Invoke(this, new OnShootEventArgs
        {
            TargetUnit = _targetUnit,
            ShootingUnit = _unit
        });

        OnShoot?.Invoke(this, new OnShootEventArgs
        {
            TargetUnit = _targetUnit,
            ShootingUnit = _unit
        });

        _targetUnit.TakeDamage(Damage);
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
                ActionComplete();
                break;
        }

        //Debug.Log(_currentState);
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        GridPosition unitGridPosition = _unit.GetGridPosition();
        return GetValidActionGridPositionList(unitGridPosition);
    }

    public List<GridPosition> GetValidActionGridPositionList(GridPosition unitGridPosition)
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        for (int x = -MaxShootRange; x <= MaxShootRange; x++)
        {
            for (int z = -MaxShootRange; z <= MaxShootRange; z++)
            {
                for (int floor = -MaxShootRange; floor <= MaxShootRange; floor++)
                {
                    GridPosition offsetGridPosition = new GridPosition(x, z, 0);
                    GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                    if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                    {
                        // grid position with x and z >= 0 and within width and height
                        continue;
                    }

                    int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                    if (testDistance > MaxShootRange)
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

                    Vector3 unitWorldPosition = LevelGrid.Instance.GetWorldPosition(unitGridPosition);
                    Vector3 shootDirection = (targetUnit.GetWorldPosition() - unitWorldPosition).normalized;

                    float unitShoulderHeight = 1.5f;
                    if (Physics.Raycast(
                        unitWorldPosition + Vector3.up * unitShoulderHeight,
                        shootDirection,
                        Vector3.Distance(unitWorldPosition, targetUnit.GetWorldPosition()),
                        _obstaclesLayerMask))
                    {
                        // Blocked by obstacle
                        continue;
                    }

                    //Debug.Log(testGridPosition);
                    validGridPositionList.Add(testGridPosition);
                }
            }
        }

        return validGridPositionList;
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        _targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);
        //Debug.Log("Aiming");

        _currentState = State.Aiming;
        _stateTimer = _aimingStateTime;

        _canShootBullet = true;

        ActionStart(onActionComplete);
    }

    public override string GetActionName()
    {
        return ActionName;
    }

    public Unit GetTargetUnit()
    {
        return _targetUnit;
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);

        return new EnemyAIAction
        {
            GridPosition = gridPosition,
            ActionValue = 100 + Mathf.RoundToInt((1 - targetUnit.GetHealthNormalized()) * 100f),
        };
    }

    public int GetTargetCountAtPosition(GridPosition gridPosition)
    {
        return GetValidActionGridPositionList(gridPosition).Count;
    }
}
