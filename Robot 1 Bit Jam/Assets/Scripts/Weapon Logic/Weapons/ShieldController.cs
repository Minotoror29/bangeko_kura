using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ShieldState { Active, Protecting, Inactive }

public class ShieldController : Weapon
{
    [SerializeField] private float cooldown = 7f;
    [SerializeField] private float protectionTime = 1f;
    private float _cooldownTimer;
    private float _protectionTimer;

    private ShieldState _currentState;

    public override void Initialize(Controller controller, HealthSystem healthSystem)
    {
        base.Initialize(controller, healthSystem);

        _cooldownTimer = 0f;
        _protectionTimer = 0f;

        _currentState = ShieldState.Active;
        healthSystem.OnDamage += TakeDamage;
    }

    public void TakeDamage(Transform damageSource)
    {
        HealthSystem.PreventedDamage = true;

        if (_currentState == ShieldState.Active)
        {
            _currentState = ShieldState.Protecting;
        }
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        if (_currentState == ShieldState.Protecting)
        {
            if (_protectionTimer < protectionTime)
            {
                _protectionTimer += Time.deltaTime;
            } else
            {
                HealthSystem.OnDamage -= TakeDamage;
                _currentState = ShieldState.Inactive;
                _protectionTimer = 0f;
                gameObject.SetActive(false);
            }
        } else if (_currentState == ShieldState.Inactive)
        {
            if (_cooldownTimer < cooldown)
            {
                _cooldownTimer += Time.deltaTime;
            } else
            {
                HealthSystem.OnDamage += TakeDamage;
                _currentState = ShieldState.Active;
                _cooldownTimer = 0f;
                gameObject.SetActive(true);
            }
        }
    }
}
