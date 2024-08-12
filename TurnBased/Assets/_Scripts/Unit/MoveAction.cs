using System.Collections.Generic;
using UnityEngine;

public class MoveAction : MonoBehaviour
{

    [SerializeField] private Animator _animator;

    [SerializeField] private float _moveSpeed = 4f;
    [SerializeField] private float _rotateSpeed = 4f;
    [SerializeField] private int _maxMoveDistance = 4;

    public float MoveSpeed { get { return _moveSpeed; } set { _moveSpeed = value; } }
    public float RotateSpeed { get { return _rotateSpeed; } set { _rotateSpeed = value; } }

    private Vector3 _targetPosition;
    private float _stoppingDistance = 0.1f;

    private Unit _unit;

    private void Awake()
    {
        _unit = GetComponent<Unit>();
        _targetPosition = transform.position;
    }

    private void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        if (Vector3.Distance(transform.position, _targetPosition) > _stoppingDistance)
        {
            Vector3 moveDirection = (_targetPosition - transform.position).normalized;
            transform.position += moveDirection * MoveSpeed * Time.deltaTime;

            transform.forward = Vector3.Lerp(transform.forward, moveDirection, RotateSpeed * Time.deltaTime);

            _animator.SetBool("IsWalking", true);
        }
        else
        {
            _animator.SetBool("IsWalking", false);
        }
    }

    public void Move(GridPosition gridPosition)
    {
        this._targetPosition = LevelGrid.Instance.GetWorldPosition(gridPosition);
    }

    public bool IsValidActionGridPosition(GridPosition gridPosition)
    {
        List<GridPosition> validGridPositionList = GetValidActionGridPositionList();
        return validGridPositionList.Contains(gridPosition);
    }

    public List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        GridPosition unitGridPosition = _unit.GetGridPosition();

        for (int x = -_maxMoveDistance; x <= _maxMoveDistance; x++)
        {
            for (int z = -_maxMoveDistance; z <= _maxMoveDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    // grid position with x and z >= 0 and within width and height
                    continue;
                }

                if (unitGridPosition == testGridPosition)
                {
                    // Same grid position where the unit is already at
                    continue;
                }

                if (LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))
                {
                    // There is a unit on this position = the grid obj list is not empty = count > 0
                    continue;
                }

                Debug.Log(testGridPosition);
                validGridPositionList.Add(testGridPosition);
            }
        }

        return validGridPositionList;
    }
}
