using FMOD.Studio;
using FMODUnity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerController : Controller
{
    private PlayerState _currentState;

    private PlayerControls _controls;

    [Header("Movement")]
    [SerializeField] private float movementSpeed = 325f;

    [Header("Laser")]
    [SerializeField] private Transform laserFirePoint;
    [SerializeField] private float laserMaxDistance = 50f;
    [SerializeField] private float laserWidth = 0.25f;
    [SerializeField] private Laser laserPrefab;
    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private int laserDamage = 10;
    [SerializeField] private float laserCooldown = 2f;
    [SerializeField] private float laserKnockbackDistance = 2f;
    [SerializeField] private float laserKnockbackSpeed = 1000f;
    [SerializeField] private Transform aim;
    [SerializeField] private GameObject laserOriginEffect;
    private float _laserCooldownTimer;
    private Vector2 _lookDirection;
    private Vector2 _mousePosition;
    private Knockback _laserKnockback;

    [Header("Fall")]
    [SerializeField] private GameObject fallSpritePrefab;
    [SerializeField] private GameObject fallDownSpritePrefab;
    private GameObject _fallSprite;
    private GameObject _fallDownSprite;

    [Header("Land")]
    [SerializeField] private GameObject landMeshPrefab;
    [SerializeField] private GameObject landEffect;
    [SerializeField] private float landEffectLifetime = 0.3f;
    [SerializeField] private int landDamage = 3;
    [SerializeField] private float landDamageRadius = 2.25f;
    [SerializeField] private float landKnockbackDistance = 3f;
    [SerializeField] private float landKnockbackSpeed = 1000f;
    [SerializeField] private float landCameraShakeGain = 5f;
    [SerializeField] private float landCameraShakeTime = 0.1f;
    [SerializeField] private float landTime;
    [SerializeField] private float landEffectTime;
    private GameObject _landMesh;
    private Knockback _landKnockback;

    private EventInstance _laserSound;
    private EventInstance _laserReloadSound;
    private EventInstance _damageSound;
    private EventInstance _lowLifeLoop;
    private EventInstance _landingSound;
    private EventInstance _fallingSound;

    public event Action OnTakeDamage;

    public PlayerState CurrentState { get { return _currentState; } }
    public PlayerControls Controls { get { return _controls; } }
    public float MovementSpeed { get { return movementSpeed; } }
    public Vector2 LookDirection { get { return _lookDirection; } }
    public float LaserCooldownTimer { get { return _laserCooldownTimer; } }
    public GameObject FallSprite { get { return _fallSprite; } }
    public GameObject FallDownSprite { get { return _fallDownSprite; } }
    public GameObject LandMesh { get { return _landMesh; } }
    public GameObject LandEffect { get { return landEffect; } }
    public float LandEffectLifetime { get { return landEffectLifetime; } }
    public int LandDamage { get { return landDamage; } }
    public float LandDamageRadius { get { return landDamageRadius; } }
    public Knockback LandKnockback { get { return _landKnockback; } }
    public float LandCameraShakeGain { get { return landCameraShakeGain; } }
    public float LandCameraShakeTime { get { return landCameraShakeTime; } }
    public float LandTime { get { return landTime; } }
    public float LandEffectTime { get { return landEffectTime; } }
    public EventInstance LandingSound { get { return _landingSound; } }
    public EventInstance FallingSound { get { return _fallingSound; } }

    public override void Initialize(GameManager gameManager)
    {
        base.Initialize(gameManager);

        _controls = new PlayerControls();
        _controls.InGame.Enable();
        _controls.InGame.Laser.performed += ctx => FireLaser();

        HealthSystem.OnHit += TakeHit;
        HealthSystem.OnDamage += TakeDamage;
        HealthSystem.OnDeath += Die;
        HealthSystem.OnDeathFromFall += DieFromFall;
        gameManager.HealthDisplay.Initialize(HealthSystem);

        _laserCooldownTimer = 0f;
        _laserKnockback = new Knockback { knockbackDistance = laserKnockbackDistance, knockbackSpeed = laserKnockbackSpeed };

        _fallSprite = Instantiate(fallSpritePrefab);
        _fallSprite.SetActive(false);
        _fallDownSprite = Instantiate(fallDownSpritePrefab);
        _fallDownSprite.SetActive(false);

        _landMesh = Instantiate(landMeshPrefab);
        _landMesh.SetActive(false);
        _landKnockback = new Knockback { knockbackDistance = landKnockbackDistance, knockbackSpeed = landKnockbackSpeed };

        _laserSound = RuntimeManager.CreateInstance("event:/Weapons/Laser");
        _laserReloadSound = RuntimeManager.CreateInstance("event:/Weapons/Laser Reload");
        _damageSound = RuntimeManager.CreateInstance("event:/Weapons/Player Hit");
        _lowLifeLoop = RuntimeManager.CreateInstance("event:/LowLifeLoop");
        _landingSound = RuntimeManager.CreateInstance("event:/Movement/Landing");
        _fallingSound = RuntimeManager.CreateInstance("event:/Movement/Fall");
    }

    private void OnDisable()
    {
        _controls.InGame.Laser.performed -= ctx => FireLaser();
    }

    public virtual void UnsubscribeEvents()
    {
        _controls.InGame.Laser.performed -= ctx => FireLaser();
        _controls.InGame.Disable();
    }

    public void ChangeState(PlayerState nextState)
    {
        _currentState?.Exit();
        _currentState = nextState;
        _currentState.Enter();
    }

    public virtual void FireLaser()
    {
        if (_laserCooldownTimer > 0f) return;

        //Physics
        Ray2D ray = new(laserFirePoint.position, _mousePosition - (Vector2)laserFirePoint.position);
        float rayDistance = laserMaxDistance;
        //Checks obstacles
        RaycastHit2D obstacleHit = Physics2D.Raycast(ray.origin, ray.direction, rayDistance, obstacleLayer);
        if (obstacleHit.collider != null)
        {
            rayDistance = obstacleHit.distance;
            if (obstacleHit.collider.TryGetComponent(out Switch s))
            {
                s.Activate();
            }
        }
        //Creates the damaging ray
        RaycastHit2D[] hits = Physics2D.BoxCastAll(ray.origin, new Vector2(laserWidth, 1), aim.rotation.eulerAngles.y, ray.direction, rayDistance, HealthSystemLayer);
        foreach (RaycastHit2D enemyHit in hits)
        {
            if (enemyHit.collider.TryGetComponent(out HealthSystem healthSystem))
            {
                if (healthSystem.Source != transform)
                {
                    healthSystem.TakeDamage(laserDamage, transform, _laserKnockback);
                }
            }
        }

        ////Visuals
        Laser newLaser = Instantiate(laserPrefab);
        newLaser.Initialize((Vector2)laserFirePoint.position + _lookDirection.normalized * 0.75f, (Vector2)laserFirePoint.position + (_mousePosition - (Vector2)laserFirePoint.position).normalized * rayDistance, laserWidth);

        InstantiateEffect(laserOriginEffect, laserFirePoint.position, Quaternion.LookRotation(Vector3.forward, _lookDirection), 0.367f);

        _laserReloadSound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        _laserSound.start();

        CameraManager.Instance.ShakeCamera(2f, 0.1f);

        _laserCooldownTimer = laserCooldown;
    }

    private void TakeHit(Transform damageSource, Knockback knockback)
    {
        _damageSound.start();

        OnTakeDamage?.Invoke();

        if (!_currentState.CanBeKnockbacked()) return;

        ChangeState(new PlayerKnockbackState(this, (transform.position - damageSource.position).normalized, knockback));
    }

    private void TakeDamage(int damage)
    {
        if (HealthSystem.CurrentHealth <= 3)
        {
            _lowLifeLoop.start();
        }
    }

    public void Die(HealthSystem healthSystem, Transform deathSource)
    {
        _lowLifeLoop.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);

        ChangeState(new PlayerDeathState(this, false));
    }

    public void DieFromFall()
    {
        _lowLifeLoop.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);

        ChangeState(new PlayerDeathState(this, true));
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        _currentState.UpdateLogic();

        HandleRotationInput();

        if (_laserCooldownTimer > 0f)
        {
            _laserCooldownTimer -= Time.deltaTime;

            if (_laserCooldownTimer <= 0f)
            {
                _laserReloadSound.start();
            }
        }
    }

    public virtual void UpdateWeapons()
    {

    }

    private void HandleRotationInput()
    {
        _mousePosition = Camera.main.ScreenToWorldPoint(_controls.InGame.MousePosition.ReadValue<Vector2>());
        _lookDirection = _mousePosition - (Vector2)laserFirePoint.localPosition - (Vector2)transform.position;
    }

    public void RotateAim()
    {
        Quaternion aimRotation = Quaternion.LookRotation(new Vector3(_lookDirection.x, 0f, _lookDirection.y), aim.up);
        aim.localRotation = Quaternion.Euler(new Vector3(0f, aimRotation.eulerAngles.y, 0f));
    }

    public virtual void RotateMesh()
    {
        Quaternion meshRotation = Quaternion.LookRotation(new Vector3(_lookDirection.x, 0f, _lookDirection.y), Mesh.up);
        Mesh.localRotation = Quaternion.Euler(new Vector3(0f, meshRotation.eulerAngles.y, 0f));
    }

    public virtual void RotateMeshSmooth(Vector3 direction, float speed)
    {
        Quaternion meshRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.y), Mesh.up);
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, meshRotation.eulerAngles.y, 0f));
        Mesh.localRotation = Quaternion.RotateTowards(Mesh.localRotation, targetRotation, speed * Time.deltaTime);
    }

    public override void UpdatePhysics()
    {
        _currentState.UpdatePhysics();
    }

    public virtual void Move(Vector2 direction, float speed)
    {
        Rb.velocity = speed * Time.fixedDeltaTime * direction;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        _currentState.OnCollisionEnter(collision);
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

        _currentState.OnTriggerEnter(collision);

        if (collision.TryGetComponent(out ScreenExit exit))
        {
            exit.ChangeScreen();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        _currentState.OnTriggerStay(collision);
    }
}
