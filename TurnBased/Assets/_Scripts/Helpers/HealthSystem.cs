using System;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{

    [SerializeField] private int _maxHealth = 100;
    [SerializeField] private int _currentHealth;
    public int CurrentHealth { get { return _currentHealth; } set { _currentHealth = value; } }
    public int MaxHealth { get { return _maxHealth; } set { _maxHealth = value; } }

    public event EventHandler OnDead;
    public event EventHandler OnDamaged;

    private void Awake()
    {
        CurrentHealth = MaxHealth;
    }

    public void TakeDamage(int damageAmount)
    {
        CurrentHealth -= damageAmount;

        if (CurrentHealth < 0)
        {
            CurrentHealth = 0;
        }

        OnDamaged?.Invoke(this, EventArgs.Empty);

        if (CurrentHealth == 0)
        {
            Die();
        }
    }

    private void Die()
    {
        OnDead?.Invoke(this, EventArgs.Empty);
    }

    public float GetHealthNormalized()
    {
        return (float)CurrentHealth / MaxHealth;
    }
}
