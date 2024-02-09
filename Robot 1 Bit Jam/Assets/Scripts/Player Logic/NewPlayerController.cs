using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements.Experimental;

public class NewPlayerController : Controller
{
    private PlayerControls _controls;

    private PlayerState _currentState;

    [Header("Movement")]
    [SerializeField] private float movementSpeed = 500f;

    [Header("Dash")]
    [SerializeField] private float dashSpeed = 1000f;
    [SerializeField] private float dashDistance = 5f;
    [SerializeField] private float dashCooldown = 2f;
    private float _dashCooldownTimer;

    [Header("Laser")]
    [SerializeField] private Transform laserFirePoint;
    [SerializeField] private float laserMaxDistance = 10f;
    [SerializeField] private float laserWidth;
    [SerializeField] private Laser laserPrefab;
    [SerializeField] private LayerMask healthSystemLayer;
    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private int laserDamage = 15;
    [SerializeField] private float laserCooldown = 3f;
    [SerializeField] private Transform mesh;
    private float _laserCooldownTimer;
    private Vector2 _lookDirection;
    private Vector2 _mousePosition;

    [Header("Weapons")]
    [SerializeField] private List<Weapon> weapons;

    public event Action OnDash;
    public event Action OnTakeDamage;

    public PlayerControls Controls { get { return _controls; } }
    public float MovementSpeed { get { return movementSpeed; } }
    public float DashSpeed { get { return dashSpeed; } }
    public float DashDistance { get { return dashDistance; } }

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

        _controls = new PlayerControls();
        _controls.InGame.Enable();
        _controls.InGame.Dash.performed += ctx => Dash();
        _controls.InGame.Laser.performed += ctx => FireLaser();

        HealthSystem.OnHit += TakeDamage;
        HealthSystem.OnDeath += Die;

        _dashCooldownTimer = 0f;
        _laserCooldownTimer = 0f;

        foreach (Weapon weapon in weapons)
        {
            weapon.Initialize(this, HealthSystem);
        }

        ChangeState(new PlayerSpawnState(this));
    }

    public void ChangeState(PlayerState nextState)
    {
        _currentState?.Exit();
        _currentState = nextState;
        _currentState.Enter();
    }

    private void Dash()
    {
        if (_dashCooldownTimer > 0f) return;

        OnDash?.Invoke();
        _dashCooldownTimer = dashCooldown;
    }

    private void FireLaser()
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
        RaycastHit2D[] hits = Physics2D.BoxCastAll(ray.origin, new Vector2(laserWidth, 1), mesh.rotation.eulerAngles.y, ray.direction, rayDistance, healthSystemLayer);
        foreach (RaycastHit2D enemyHit in hits)
        {
            if (enemyHit.collider.TryGetComponent(out HealthSystem healthSystem))
            {
                if (healthSystem.Source != transform)
                {
                    healthSystem.TakeDamage(laserDamage, transform);
                }
            }
        }

        ////Visuals
        Laser newLaser = Instantiate(laserPrefab);
        newLaser.Initialize(laserFirePoint.position, (Vector2)laserFirePoint.position + (_mousePosition - (Vector2)laserFirePoint.position).normalized * rayDistance, laserWidth);

        _laserCooldownTimer = laserCooldown;
    }

    private void TakeDamage(Transform damageSource)
    {
        OnTakeDamage?.Invoke();
    }

    public void Die(HealthSystem healthSystem, Transform deathSource)
    {
        _controls.InGame.Dash.performed -= ctx => Dash();
        _controls.InGame.Laser.performed -= ctx => FireLaser();
        _controls.InGame.Disable();

        HealthSystem.OnHit -= TakeDamage;
        HealthSystem.OnDeath -= Die;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public override void UpdateLogic()
    {
        _currentState.UpdateLogic();

        foreach (Weapon weapon in weapons)
        {
            weapon.UpdateLogic();
        }

        HandleRotationInput();

        Rotate();

        if (_dashCooldownTimer > 0f)
        {
            _dashCooldownTimer -= Time.deltaTime;
        }

        if (_laserCooldownTimer > 0f)
        {
            _laserCooldownTimer -= Time.deltaTime;
        }

        //Animator.SetFloat("X Direction", mesh.transform.InverseTransformDirection(Rb.velocity).x);
        //Animator.SetFloat("Y Direction", mesh.transform.InverseTransformDirection(Rb.velocity).z);
    }

    private void HandleRotationInput()
    {
        _mousePosition = Camera.main.ScreenToWorldPoint(_controls.InGame.MousePosition.ReadValue<Vector2>());
        _lookDirection = _mousePosition - (Vector2)laserFirePoint.localPosition - (Vector2)transform.position;
    }

    private void Rotate()
    {
        Quaternion meshRotation = Quaternion.LookRotation(new Vector3(_lookDirection.x, 0f, _lookDirection.y), mesh.up);
        mesh.localRotation = Quaternion.Euler(new Vector3(0f, meshRotation.eulerAngles.y, 0f));
    }

    public override void UpdatePhysics()
    {
        _currentState.UpdatePhysics();

        foreach (Weapon weapon in weapons)
        {
            weapon.UpdatePhysics();
        }

        //Move();
    }

    public void Move(Vector2 direction, float speed)
    {
        Rb.velocity = speed * Time.fixedDeltaTime * direction;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        _currentState.OnCollisionEnter(collision);
    }
}
