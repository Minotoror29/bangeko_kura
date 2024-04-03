using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ShieldState { Active, Protecting, Inactive }

public class ShieldController : Weapon
{
    [SerializeField] private GameObject mesh;
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

    public void TakeDamage(Transform damageSource, Knockback knockback)
    {
        if (!gameObject.activeSelf)
        {
            return;
        }

        HealthSystem.PreventDamage = true;

        if (_currentState == ShieldState.Active)
        {
            _currentState = ShieldState.Protecting;
        }
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        if (!gameObject.activeSelf)
        {
            return;
        }

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
                mesh.SetActive(false);
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
                mesh.SetActive(true);
            }
        }
    }
}
