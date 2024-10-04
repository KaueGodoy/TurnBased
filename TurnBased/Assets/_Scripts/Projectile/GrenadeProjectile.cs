using System;
using UnityEngine;

public class GrenadeProjectile : MonoBehaviour
{
    public static event EventHandler OnAnyGrenadeExploded;

    [SerializeField] private Transform _grenadeExplosionVFXPrefab;
    [SerializeField] private Transform _trailRenderer;
    [SerializeField] private AnimationCurve _arcYAnimationCurve;

    [SerializeField] private float _moveSpeed = 15f;
    public float MoveSpeed { get { return _moveSpeed; } set { _moveSpeed = value; } }

    [SerializeField] private float _damageRadius = 4f;
    public float DamageRadius { get { return _damageRadius; } set { _damageRadius = value; } }

    [SerializeField] private int _damageAmount = 30;
    public int DamageAmount { get { return _damageAmount; } set { _damageAmount = value; } }

    private Action _onGrenadeBehaviourComplete;
    private Vector3 _targetPosition;

    private Vector3 _positionXZ;
    private float _totalDistance;

    private void Update()
    {
        Vector3 moveDir = (_targetPosition - _positionXZ).normalized;

        _positionXZ += moveDir * MoveSpeed * Time.deltaTime;

        float distance = Vector3.Distance(_positionXZ, _targetPosition);
        float distanceNormalized = 1 - distance / _totalDistance;

        float positionY = _arcYAnimationCurve.Evaluate(distanceNormalized);

        transform.position = new Vector3(_positionXZ.x, positionY, _positionXZ.z);

        float reachedTargetDistance = .2f;
        if (Vector3.Distance(_positionXZ, _targetPosition) < reachedTargetDistance)
        {
            Collider[] colliderArray = Physics.OverlapSphere(_targetPosition, DamageRadius);

            foreach (Collider collider in colliderArray)
            {
                if (collider.TryGetComponent<Unit>(out Unit targetUnit))
                {
                    targetUnit.TakeDamage(DamageAmount);
                }
            }

            OnAnyGrenadeExploded?.Invoke(this, EventArgs.Empty);

            _trailRenderer.transform.parent = null;

            float offset = 1f;
            Instantiate(_grenadeExplosionVFXPrefab, _targetPosition + Vector3.up * offset, Quaternion.identity);

            Destroy(gameObject);

            _onGrenadeBehaviourComplete();
        }
    }


    public void Setup(GridPosition targetGridPosition, Action onGrenadeBehaviourComplete)
    {
        this._onGrenadeBehaviourComplete = onGrenadeBehaviourComplete;
        _targetPosition = LevelGrid.Instance.GetWorldPosition(targetGridPosition);

        _positionXZ = transform.position;
        _positionXZ.y = 0;

        _totalDistance = Vector3.Distance(_positionXZ, _targetPosition);
    }
}
