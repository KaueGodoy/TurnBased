using System;
using UnityEngine;

public class GrenadeProjectile : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 15f;
    public float MoveSpeed { get { return _moveSpeed; } set { _moveSpeed = value; } }

    [SerializeField] private float _damageRadius = 4f;
    public float DamageRadius { get { return _damageRadius; } set { _damageRadius = value; } }

    [SerializeField] private int _damageAmount = 30;
    public int DamageAmount { get { return _damageAmount; } set { _damageAmount = value; } }

    private Action _onGrenadeBehaviourComplete;
    private Vector3 _targetPosition;

    private void Update()
    {
        Vector3 moveDir = (_targetPosition - transform.position).normalized;

        transform.position += moveDir * MoveSpeed * Time.deltaTime;

        float reachedTargetDistance = .2f;
        if (Vector3.Distance(transform.position, _targetPosition) < reachedTargetDistance)
        {
            Collider[] colliderArray = Physics.OverlapSphere(_targetPosition, DamageRadius);

            foreach (Collider collider in colliderArray)
            {
                if (collider.TryGetComponent<Unit>(out Unit targetUnit))
                {
                    targetUnit.TakeDamage(DamageAmount);
                }
            }

            Destroy(gameObject);

            _onGrenadeBehaviourComplete();
        }
    }


    public void Setup(GridPosition targetGridPosition, Action onGrenadeBehaviourComplete)
    {
        this._onGrenadeBehaviourComplete = onGrenadeBehaviourComplete;
        _targetPosition = LevelGrid.Instance.GetWorldPosition(targetGridPosition);
    }
}
