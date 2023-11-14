using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    private Transform _controller;
    private HealthSystem _healthSystem;

    public Transform Controller { get { return _controller; } }
    public HealthSystem HealthSystem { get { return _healthSystem; } }

    public virtual void Initialize(Transform controller, HealthSystem healthSystem)
    {
        _controller = controller;
        _healthSystem = healthSystem;
    }

    public virtual void UpdateLogic()
    {

    }
}
