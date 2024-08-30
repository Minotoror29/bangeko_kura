using FMOD.Studio;
using FMODUnity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalkController : PlayerController
{
    [Header("Dash")]
    [SerializeField] private float dashSpeed = 1500f;
    [SerializeField] private float dashDistance = 6f;
    [SerializeField] private float dashCooldown = 0.5f;
    [SerializeField] private GameObject dashEffect;
    private float _dashCooldownTimer;

    [Header("Weapons")]
    [SerializeField] private TurretController turret;
    [SerializeField] private SwordController sword;
    
    //Audio
    private EventInstance _dashSound;
    private EventInstance _stepSound;    

    #region Getters / Setters
    public float DashSpeed { get { return dashSpeed; } }
    public float DashDistance { get { return dashDistance; } }
    public GameObject DashEffect { get { return dashEffect; } }
    public EventInstance DashSound { get { return _dashSound; } }
    #endregion

    public override void Initialize(GameManager gameManager)
    {
        base.Initialize(gameManager);

        Controls.InGame.Dash.performed += ctx => Dash();
        Controls.InGame.Sword.performed += ctx => sword.SwordStrike();

        _dashCooldownTimer = 0f;

        turret.Initialize(this, HealthSystem);
        sword.Initialize(this, HealthSystem);        

        //Initialize Audio
        _dashSound = RuntimeManager.CreateInstance("event:/Movement/Dash");
        _stepSound = RuntimeManager.CreateInstance("event:/Movement/Step");
    }

    private void OnDisable()
    {
        Controls.InGame.Dash.performed -= ctx => Dash();
        Controls.InGame.Sword.performed -= ctx => sword.SwordStrike();
    }

    public override void UnsubscribeEvents()
    {
        base.UnsubscribeEvents();

        Controls.InGame.Dash.performed -= ctx => Dash();
        Controls.InGame.Sword.performed -= ctx => sword.SwordStrike();
    }

    private void Dash()
    {
        if (_dashCooldownTimer > 0f) return;

        if (CurrentState.CanDash())
        {
            _dashCooldownTimer = dashCooldown;
        }
    }

    public override void SetCollidersActive(bool active)
    {
        base.SetCollidersActive(active);

        turret.SetActiveColliders(active);
    }

    public override bool SwordAttack(float builupTime)
    {
        return CurrentState.CanAttackSword();
    }

    public void PlayStepSound()
    {
        _stepSound.start();
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        if (_dashCooldownTimer > 0f)
        {
            _dashCooldownTimer -= Time.deltaTime;
        }
    }

    public override void UpdateWeapons()
    {
        base.UpdateWeapons();

        turret.UpdateLogic();
        sword.UpdateLogic();
    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();

        turret.UpdatePhysics();
        sword.UpdatePhysics();
    }
}
