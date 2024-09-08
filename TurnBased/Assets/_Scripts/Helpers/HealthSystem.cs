using System;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{

    [SerializeField] private int _currentHealth = 100;
    public int CurrentHealth { get { return _currentHealth; } set { _currentHealth = value; } }

    public event EventHandler OnDead;

    public void TakeDamage(int damageAmount)
    {
        CurrentHealth -= damageAmount;

        if (CurrentHealth < 0)
        {
            CurrentHealth = 0;
        }

        if (CurrentHealth == 0)
        {
            Die();
        }
    }

    private void Die()
    {
        OnDead?.Invoke(this, EventArgs.Empty);
    }
}
