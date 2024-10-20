using System;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeAction : BaseAction
{
    [SerializeField] private Transform _grenadeProjectilePrefab;

    [SerializeField] private int _maxThrowDistance = 7;
    public int MaxThrowDistance { get { return _maxThrowDistance; } set { _maxThrowDistance = value; } }

    protected override void Awake()
    {
        base.Awake();
        ActionName = "Grenade";
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

        for (int x = -MaxThrowDistance; x <= MaxThrowDistance; x++)
        {
            for (int z = -MaxThrowDistance; z <= MaxThrowDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z, 0);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    // grid position with x and z >= 0 and within width and height
                    continue;
                }

                int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if (testDistance > MaxThrowDistance)
                {
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
        Transform grenadeProjectileTransform = Instantiate(_grenadeProjectilePrefab, _unit.GetWorldPosition(), Quaternion.identity);
        GrenadeProjectile grenadeProjectile = grenadeProjectileTransform.GetComponent<GrenadeProjectile>();
        grenadeProjectile.Setup(gridPosition, OnGrenadeActionCompleted);

        ActionStart(onActionComplete);
    }

    private void OnGrenadeActionCompleted()
    {
        ActionComplete();
    }
}
