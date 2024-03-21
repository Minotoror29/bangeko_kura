using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    private Controller _controller;
    private HealthSystem _healthSystem;

    public Controller Controller { get { return _controller; } }
    public HealthSystem HealthSystem { get { return _healthSystem; } }

    public virtual void Initialize(Controller controller, HealthSystem healthSystem)
    {
        _controller = controller;
        _healthSystem = healthSystem;
    }

    public virtual void UpdateLogic()
    {

    }

    public virtual void UpdatePhysics()
    {

    }
}
