using UnityEngine;

public class Unit : MonoBehaviour
{

    private Vector3 _targetPosition;

    [SerializeField] private float _moveSpeed = 4f;
    public float MoveSpeed { get { return _moveSpeed; } set { _moveSpeed = value; } }

    private float _stoppingDistance = 0.1f;

    private void Update()
    {
        if (Vector3.Distance(transform.position, _targetPosition) > _stoppingDistance)
        {
            Vector3 moveDirection = (_targetPosition - transform.position).normalized;
            transform.position += moveDirection * MoveSpeed * Time.deltaTime;
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
