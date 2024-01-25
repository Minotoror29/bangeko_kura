using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ShieldState { Active, Protecting, Inactive }

public class ShieldController : Weapon
{
    [SerializeField] private float cooldown = 7f;
    [SerializeField] private float protectionTime = 0.2f;
    private float _cooldownTimer;
    private float _protectionTimer;

    private ShieldState _currentState;

    private EventInstance _shieldSound;

    public override void Initialize(Controller controller, HealthSystem healthSystem)
    {
        base.Initialize(controller, healthSystem);

        _cooldownTimer = 0f;
        _protectionTimer = 0f;

        _currentState = ShieldState.Active;
        healthSystem.OnHit += TakeDamage;

        _shieldSound = RuntimeManager.CreateInstance("event:/Weapons/Shield");
    }

    public void TakeDamage(Transform damageSource)
    {
        HealthSystem.PreventDamage = true;

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
                HealthSystem.OnHit -= TakeDamage;
                _currentState = ShieldState.Inactive;
                _protectionTimer = 0f;
                gameObject.SetActive(false);
                _shieldSound.start();
            }
        } else if (_currentState == ShieldState.Inactive)
        {
            if (_cooldownTimer < cooldown)
            {
                _cooldownTimer += Time.deltaTime;
            } else
            {
                HealthSystem.OnHit += TakeDamage;
                _currentState = ShieldState.Active;
                _cooldownTimer = 0f;
                gameObject.SetActive(true);
            }
        }
    }
}
