using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        _playerControls = new PlayerControls();
        _playerControls.InGame.Enable();
        _playerControls.InGame.Laser.performed += ctx => FireLaser();
        _playerControls.InGame.Dash.performed += ctx => Dash();

        _healthSystem = GetComponent<HealthSystem>();
        _healthSystem.Initialize();

        _laserCooldownTimer = laserCooldown;
        _dashCooldownTimer = dashCooldown;

        foreach (Weapon weapon in weapons)
        {
            weapon.Initialize(this, _healthSystem);
        }
    }

    public void UpdateLogic()
    {
        foreach (Weapon weapon in weapons)
        {
            weapon.UpdateLogic();
        }

        HandleMovementInput();
        HandleLookInput();

        Look();

        if (_laserCooldownTimer < laserCooldown)
        {
            _laserCooldownTimer += Time.deltaTime;
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
                Dashing = false;
                _dashCooldownTimer = 0f;
            }
        }
    }

    private void HandleMovementInput()
    {
        _movementDirection.x = _playerControls.InGame.Movement.ReadValue<Vector2>().x;
        _movementDirection.z = _playerControls.InGame.Movement.ReadValue<Vector2>().y;
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

    public void UpdatePhysics()
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
        newLaser.Initialize(laserFirePoint.position, laserFirePoint.position + transform.forward * rayDistance);

        _laserCooldownTimer = 0f;
    }

    private void Dash()
    {
        if (_dashCooldownTimer < dashCooldown || _movementDirection.magnitude == 0f) return;

        _dashTimer = 0f;
        _dashDirection = _movementDirection;
        Dashing = true;
    }
}
