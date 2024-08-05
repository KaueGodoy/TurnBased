using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    [SerializeField] private float _moveSpeed = 4f;
    [SerializeField] private float _rotateSpeed = 4f;



    public float MoveSpeed { get { return _moveSpeed; } set { _moveSpeed = value; } }
    public float RotateSpeed { get { return _rotateSpeed; } set { _rotateSpeed = value; } }

    private float _stoppingDistance = 0.1f;

    private Vector3 _targetPosition;
    private GridPosition _gridPosition;

    private void Awake()
    {
        _targetPosition = transform.position;
    }

    private void Start()
    {
        _gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.SetUnitAtGridPosition(_gridPosition, this);
    }

    private void Update()
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

        GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);

        if (newGridPosition != _gridPosition)
        {

        }

    }

    public void Move(Vector3 targetPosition)
    {
        this._targetPosition = targetPosition;
    }
}
