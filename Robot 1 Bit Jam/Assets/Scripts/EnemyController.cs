using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private HealthSystem _healthSystem;

    public event Action<EnemyController> OnDeath;

    private void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        _healthSystem = GetComponent<HealthSystem>();
        _healthSystem.Initialize();
        _healthSystem.OnDeath += Die;
    }

    private void Die()
    {
        OnDeath.Invoke(this);
        Destroy(gameObject);
    }
}
