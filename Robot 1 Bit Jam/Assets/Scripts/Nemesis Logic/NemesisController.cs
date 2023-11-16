using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NemesisController : Controller
{
    private HealthSystem _healthSystem;
    private PlayerController _player;

    [SerializeField] private float movementSpeed = 1000f;

    [SerializeField] private List<Weapon> weapons;

    private NemesisState _currentState;

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
        _healthSystem.Initialize();
        _player = FindObjectOfType<PlayerController>();

        foreach (Weapon weapon in weapons)
        {
            weapon.Initialize(this, _healthSystem);
        }

        ChangeState(new NemesisFarState(this, _player));
    }

    public void ChangeState(NemesisState nextState)
    {
        _currentState?.Exit();
        _currentState = nextState;
        _currentState.Enter();
    }

    public override void UpdateLogic()
    {
        foreach (Weapon weapon in weapons)
        {
            weapon.UpdateLogic();
        }

        _currentState.UpdateLogic();
    }

    public override void UpdatePhysics()
    {
        foreach (Weapon weapon in weapons)
        {
            weapon.UpdatePhysics();
        }

        _currentState.UpdatePhysics();
    }

    public void MoveTowards(Vector3 direction)
    {
        if (!Dashing)
        {
            Rb.velocity = movementSpeed * Time.fixedDeltaTime * direction.normalized;
        }
    }
}
