using System;
using UnityEngine;

public class DestructibleCrate : MonoBehaviour
{
    [SerializeField] private Transform _crateDestroyedPrefab;
    [SerializeField] private float _explosionForce = 100f;
    [SerializeField] private float _explosionRange = 10f;

    public static event EventHandler OnAnyDestroyed;

    public GridPosition GridPosition { get; set; }


    private void Start()
    {
        GridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
    }

    public void TakeDamage()
    {
        Transform crateDestroyedTransform = Instantiate(_crateDestroyedPrefab, transform.position, Quaternion.identity);

        ApplyExplosionToChildren(crateDestroyedTransform, _explosionForce, transform.position, _explosionRange);

        Destroy(gameObject);

        OnAnyDestroyed?.Invoke(this, EventArgs.Empty);
    }

    private void ApplyExplosionToChildren(Transform root, float explosionForce, Vector3 explosionPosition, float explosionRange)
    {
        foreach (Transform child in root)
        {
            if (child.TryGetComponent<Rigidbody>(out Rigidbody childRigidBody))
            {
                childRigidBody.AddExplosionForce(explosionForce, explosionPosition, explosionRange);
            }

            ApplyExplosionToChildren(child, explosionForce, explosionPosition, explosionRange);
        }
    }
}
