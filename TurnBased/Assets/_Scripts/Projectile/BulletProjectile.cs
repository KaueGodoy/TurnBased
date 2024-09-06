using UnityEngine;

public class BulletProjectile : MonoBehaviour
{
    private Vector3 _targetPosition;

    public void Setup(Vector3 targetPosition)
    {
        this._targetPosition = targetPosition;
    }

    private void Update()
    {
        Vector3 moveDir = (_targetPosition - transform.position).normalized;

        float distanceBeforeMoving = Vector3.Distance(transform.position, _targetPosition);

        float moveSpeed = 200f;
        transform.position += moveDir * moveSpeed * Time.deltaTime;

        float distanceAfterMoving = Vector3.Distance(transform.position, _targetPosition);

        if (distanceBeforeMoving < distanceAfterMoving)
        {
            Destroy(gameObject);
        }

    }
}
