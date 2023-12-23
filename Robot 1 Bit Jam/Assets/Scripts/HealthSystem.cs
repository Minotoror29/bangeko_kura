using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] private int maxHealth;
    private int _currentHealth;

    private bool _preventDamage;

    public int CurrentHealth { get { return _currentHealth; } }
    public bool PreventDamage { get { return _preventDamage; } set { _preventDamage = value; } }

    public event Action<Transform> OnDamage;
    public event Action<HealthSystem, Transform> OnDeath;

    public void Initialize()
    {
        _currentHealth = maxHealth;

        _preventDamage = false;
    }

    public void TakeDamage(int damage, Transform damageSource)
    {
        OnDamage?.Invoke(damageSource);

        if (_preventDamage)
        {
            _preventDamage = false;
            return;
        }

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
