using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    private Transform _source;

    [SerializeField] private int maxHealth;
    private int _currentHealth;

    private bool _preventDamage;

    public Transform Source { get { return _source; } }
    public int CurrentHealth { get { return _currentHealth; } }
    public bool PreventDamage { get { return _preventDamage; } set { _preventDamage = value; } }

    public event Action<Transform> OnHit;
    public event Action OnDamage;
    public event Action<HealthSystem, Transform> OnDeath;

    public void Initialize(Transform source)
    {
        _source = source;
        _currentHealth = maxHealth;

        _preventDamage = false;
    }

    public void TakeDamage(int damage, Transform damageSource)
    {
        OnHit?.Invoke(damageSource);

        if (_preventDamage)
        {
            _preventDamage = false;
            return;
        }

        OnDamage?.Invoke();
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
