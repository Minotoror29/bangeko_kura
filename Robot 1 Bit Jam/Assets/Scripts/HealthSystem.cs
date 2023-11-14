using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] private int maxHealth;
    private int _currentHealth;

    public event Action<HealthSystem, Transform> OnDeath;

    public void Initialize()
    {
        _currentHealth = maxHealth;
    }

    public void TakeDamage(int damage, Transform damageSource)
    {
        _currentHealth -= damage;
        if (_currentHealth <= 0)
        {
            Die(damageSource);
        }
    }

    public void Die(Transform deathSource)
    {
        OnDeath?.Invoke(this, deathSource);
    }
}
