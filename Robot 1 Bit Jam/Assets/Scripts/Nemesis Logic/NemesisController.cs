using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NemesisController : Controller
{
    private HealthSystem _healthSystem;

    [SerializeField] private List<Weapon> weapons;

    private void Start()
    {
        Initialize();
    }

    private void Update()
    {
        UpdateLogic();
    }

    private void FixedUpdate()
    {
        UpdatePhysics();
    }

    public override void Initialize()
    {
        base.Initialize();

        _healthSystem = GetComponent<HealthSystem>();

        foreach (Weapon weapon in weapons)
        {
            weapon.Initialize(this, _healthSystem);
        }
    }

    public override void UpdateLogic()
    {
        foreach (Weapon weapon in weapons)
        {
            weapon.UpdateLogic();
        }
    }

    public override void UpdatePhysics()
    {
        foreach (Weapon weapon in weapons)
        {
            weapon.UpdatePhysics();
        }
    }
}
