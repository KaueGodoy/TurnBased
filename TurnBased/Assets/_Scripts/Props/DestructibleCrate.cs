using System;
using UnityEngine;

public class DestructibleCrate : MonoBehaviour
{

    public static event EventHandler OnAnyDestroyed;

    public GridPosition GridPosition { get; set; }


    private void Start()
    {
        GridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
    }

    public void TakeDamage()
    {
        Destroy(gameObject);

        OnAnyDestroyed?.Invoke(this, EventArgs.Empty);
    }
}
