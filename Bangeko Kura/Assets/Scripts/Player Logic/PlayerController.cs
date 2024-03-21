using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using FMOD.Studio;
using FMODUnity;

public class PlayerController : Controller
{
    private PlayerControls _playerControls;
    private HealthSystem _healthSystem;

    [Header("Movement")]
    [SerializeField] private float movementSpeed = 100f;
    private Vector3 _movementDirection;

    [Header("Rotation")]
    [SerializeField] LayerMask groundLayer;
    private Vector3 _lookDirection;

    [Header("Laser")]
    [SerializeField] private Transform laserFirePoint;
    [SerializeField] private float laserMaxDistance = 10f;
    [SerializeField] private Laser laserPrefab;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private int laserDamage = 15;
    [SerializeField] private float laserCooldown = 3f;
    private float _laserCooldownTimer;

    [Header("Dash")]
    [SerializeField] private float dashSpeed = 1000f;
    [SerializeField] private float dashTime = 0.1f;
    [SerializeField] private float dashCooldown = 2f;
    private float _dashTimer;
    private float _dashCooldownTimer;
    private Vector3 _dashDirection;

    [Header("Weapons")]
    [SerializeField] private List<Weapon> weapons;

    //Audio
    private EventInstance _laserSound;
    private EventInstance _laserReloadSound;
    private EventInstance _dashSound;
    private EventInstance _rollSound;

    private void Start()
    {
        //Initialize();
    }

    private void Update()
    {
        UpdateLogic();
    }

    private void FixedUpdate()
    {
        UpdatePhysics();
    }

    public override void Initialize(ScreenManager screenManager)
    {
        base.Initialize(screenManager);

        _playerControls = new PlayerControls();
        _playerControls.InGame.Enable();
        _playerControls.InGame.Laser.performed += ctx => FireLaser();
        _playerControls.InGame.Dash.performed += ctx => Dash();

        _healthSystem = GetComponent<HealthSystem>();
        _healthSystem.Initialize(transform);
        _healthSystem.OnDeath += Die;

        _laserCooldownTimer = laserCooldown;
        _dashCooldownTimer = dashCooldown;

        foreach (Weapon weapon in weapons)
        {
            weapon.Initialize(this, _healthSystem);
        }

        //Initialize audio
        _laserSound = RuntimeManager.CreateInstance("event:/Weapons/Laser");
        _laserReloadSound = RuntimeManager.CreateInstance("event:/Weapons/Laser reload");
        _dashSound = RuntimeManager.CreateInstance("event:/Movement/Dash");
        _rollSound = RuntimeManager.CreateInstance("event:/Movement/Roll");
    }

    private void Die(HealthSystem healthSystem, Transform deathSource)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public override void UpdateLogic()
    {
        foreach (Weapon weapon in weapons)
        {
            weapon.UpdateLogic();
        }

        HandleMovementInput();
        //HandleLookInput();

        //Look();

        if (_laserCooldownTimer < laserCooldown)
        {
            _laserCooldownTimer += Time.deltaTime;

            if (_laserCooldownTimer >= laserCooldown)
            {
                _laserReloadSound.start();
            }
        }

        if (_dashCooldownTimer < dashCooldown)
        {
            _dashCooldownTimer += Time.deltaTime;
        }

        if (Dashing)
        {
            if (_dashTimer < dashTime)
            {
                _dashTimer += Time.deltaTime;
            }
            else
            {
                if (Animator != null)
                {
                    Animator.SetBool("Dashing", false);
                }
                Dashing = false;
                _dashCooldownTimer = 0f;
            }
        }
    }

    private void HandleMovementInput()
    {
        _movementDirection = _playerControls.InGame.Movement.ReadValue<Vector2>();

        //if (Animator != null)
        //{
        //    if (_movementDirection.magnitude > 0)
        //    {
        //        Animator.SetBool("Walking", true);
        //    }
        //    else
        //    {
        //        Animator.SetBool("Walking", false);
        //    }
        //}
    }

    private void HandleLookInput()
    {
        Ray ray = Camera.main.ScreenPointToRay(_playerControls.InGame.MousePosition.ReadValue<Vector2>());
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundLayer))
        {
            _lookDirection = hit.point - Vector3.up * hit.point.y;

            Debug.DrawRay(Camera.main.transform.position, hit.point - Camera.main.transform.position, Color.red);
        }
    }

    private void Look()
    {
        transform.LookAt(_lookDirection);
    }

    public override void UpdatePhysics()
    {
        foreach (Weapon weapon in weapons)
        {
            weapon.UpdatePhysics();
        }

        if (!Dashing)
        {
            Rb.velocity = movementSpeed * Time.fixedDeltaTime * _movementDirection.normalized;
        } else
        {
            Rb.velocity = dashSpeed * Time.fixedDeltaTime * _dashDirection.normalized;
        }
    }

    private void FireLaser()
    {
        if (_laserCooldownTimer < laserCooldown) return;

        //Physics
        Ray ray = new(laserFirePoint.position, transform.forward);
        float rayDistance = laserMaxDistance;
        if (Physics.Raycast(ray, out RaycastHit obstacleHit, laserMaxDistance, obstacleLayer))
        {
            rayDistance = obstacleHit.distance;
        }
        RaycastHit[] hits = Physics.RaycastAll(ray, rayDistance, enemyLayer);
        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.TryGetComponent(out HealthSystem healthSystem))
            {
                healthSystem.TakeDamage(laserDamage, transform);
            }
        }

        //Visuals
        Laser newLaser = Instantiate(laserPrefab);
        newLaser.Initialize(laserFirePoint.position, laserFirePoint.position + transform.forward * rayDistance, 1);

        //Audio
        _laserSound.start();

        _laserCooldownTimer = 0f;
    }

    private void Dash()
    {
        if (_dashCooldownTimer < dashCooldown || _movementDirection.magnitude == 0f) return;

        if (Animator != null)
        {
            Animator.SetBool("Dashing", true);
        }
        _dashTimer = 0f;
        _dashDirection = _movementDirection;
        Dashing = true;

        if (dashSpeed == 1500f)
        {
            _dashSound.start();
        } else if (dashSpeed > 0f)
        {
            _rollSound.start();
        }
    }
}
