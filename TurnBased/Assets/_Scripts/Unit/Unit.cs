using System;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public static event EventHandler OnAnyActionPointsChanged;
    public static event EventHandler OnAnyUnitSpawned;
    public static event EventHandler OnAnyUnitDead;

    private GridPosition _gridPosition;

    private HealthSystem _healthSystem;
    private BaseAction[] _baseActionArray;

    [SerializeField] private bool _isEnemy = false;
    [SerializeField] private int _maxActionPoints = 2;
    [SerializeField] private int _actionPoints = 2;
    public int ActionPoints { get { return _actionPoints; } set { _actionPoints = value; } }
    public int MaxActionPoints { get { return _maxActionPoints; } set { _maxActionPoints = value; } }
    public bool IsEnemy { get { return _isEnemy; } set { _isEnemy = value; } }

    private void Awake()
    {
        _healthSystem = GetComponent<HealthSystem>();
        _baseActionArray = GetComponents<BaseAction>();
    }

    private void Start()
    {
        _gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(_gridPosition, this);

        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
        _healthSystem.OnDead += HealthSystem_OnDead;

        ActionPoints = MaxActionPoints;

        OnAnyUnitSpawned?.Invoke(this, EventArgs.Empty);
    }

    private void HealthSystem_OnDead(object sender, EventArgs e)
    {
        LevelGrid.Instance.RemoveUnitAtGridPosition(_gridPosition, this);

        Destroy(gameObject);

        OnAnyUnitDead?.Invoke(this, EventArgs.Empty);
    }

    private void Update()
    {
        GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);

        if (newGridPosition != _gridPosition)
        {
            GridPosition oldGridPosition = _gridPosition;
            _gridPosition = newGridPosition;

            LevelGrid.Instance.UnitMovedGridPosition(this, oldGridPosition, newGridPosition);
        }
    }

    public void TakeDamage(int damageAmount)
    {
        _healthSystem.TakeDamage(damageAmount);
        Debug.Log(damageAmount);
    }

    public T GetAction<T>() where T : BaseAction
    {
        foreach (BaseAction baseAction in _baseActionArray)
        {
            if (baseAction is T)
            {
                return (T)baseAction;
            }
        }
        return null;
    }

    public GridPosition GetGridPosition()
    {
        return _gridPosition;
    }

    public Vector3 GetWorldPosition()
    {
        return transform.position;
    }

    public BaseAction[] GetBaseActionArray()
    {
        return _baseActionArray;
    }

    public bool TrySpendActionPointsToTakeAction(BaseAction baseAction)
    {
        if (CanSpendActionPointsToTakeAction(baseAction))
        {
            SpendActionPoints(baseAction.GetActionPointsCost());
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool CanSpendActionPointsToTakeAction(BaseAction baseAction)
    {
        if (ActionPoints >= baseAction.GetActionPointsCost())
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void SpendActionPoints(int amount)
    {
        ActionPoints -= amount;

        OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        if ((IsEnemy && !TurnSystem.Instance.IsPlayerTurn) ||
            (!IsEnemy && TurnSystem.Instance.IsPlayerTurn))
        {
            ActionPoints = MaxActionPoints;
            OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public int GetActionPoints()
    {
        return ActionPoints;
    }

    public bool IsUnitEnemy()
    {
        return _isEnemy;
    }

    public float GetHealthNormalized()
    {
        return _healthSystem.GetHealthNormalized();
    }
}
