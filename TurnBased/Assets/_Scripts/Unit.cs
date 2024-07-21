using UnityEngine;

public class Unit : MonoBehaviour
{

    private Vector3 _targetPosition;

    [SerializeField] private Animator _animator;

    [SerializeField] private float _moveSpeed = 4f;
    public float MoveSpeed { get { return _moveSpeed; } set { _moveSpeed = value; } }

    private float _stoppingDistance = 0.1f;

    private void Start()
    {
        _targetPosition = transform.position;
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, _targetPosition) > _stoppingDistance)
        {
            Vector3 moveDirection = (_targetPosition - transform.position).normalized;
            transform.position += moveDirection * MoveSpeed * Time.deltaTime;
            
            _animator.SetBool("IsWalking", true);
        }
        else
        {
            _animator.SetBool("IsWalking", false);
        }

        if (Input.GetMouseButtonDown(0))
        {
            Move(MouseWorld.GetPosition());
        }
    }

    private void Move(Vector3 targetPosition)
    {
        this._targetPosition = targetPosition;
    }
}
