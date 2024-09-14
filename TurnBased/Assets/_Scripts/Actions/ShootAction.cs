using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShootAction : BaseAction
{
    public event EventHandler<OnShootEventArgs> OnShoot;
    public class OnShootEventArgs : EventArgs
    {
        public Unit TargetUnit { get; set; }
        public Unit ShootingUnit { get; set; }
    }

    [SerializeField] private int _maxShootRange = 4;

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
                float rotateSpeed = 10f;
                transform.forward = Vector3.Lerp(transform.forward, aimingDirection, rotateSpeed * Time.deltaTime);
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
}
