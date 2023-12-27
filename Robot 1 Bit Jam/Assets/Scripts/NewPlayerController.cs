using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements.Experimental;

public class NewPlayerController : Controller
{
    private PlayerControls _controls;
    [SerializeField] private HealthSystem healthSystem;

    [Header("Movement")]
    [SerializeField] private float movementSpeed = 500f;
    private Vector2 _movementDirection;

    [Header("Dash")]
    [SerializeField] private float dashSpeed = 1000f;
    [SerializeField] private float dashDistance = 5f;
    [SerializeField] private float dashCooldown = 2f;
    private float _dashCooldownTimer;
    private Vector3 _dashDirection;
    private Vector2 _dashOrigin;

    [Header("Laser")]
    [SerializeField] private Transform laserFirePoint;
    [SerializeField] private float laserMaxDistance = 10f;
    [SerializeField] private float laserWidth;
    [SerializeField] private Laser laserPrefab;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private int laserDamage = 15;
    [SerializeField] private float laserCooldown = 3f;
    [SerializeField] private Transform mesh;
    private float _laserCooldownTimer;
    private Vector2 _lookDirection;
    private Vector2 _mousePosition;

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

        _controls = new PlayerControls();
        _controls.InGame.Enable();
        _controls.InGame.Dash.performed += ctx => Dash();
        _controls.InGame.Laser.performed += ctx => FireLaser();

        healthSystem.Initialize();
        healthSystem.OnDamage += TakeDamage;
        healthSystem.OnDeath += Die;

        _dashCooldownTimer = dashCooldown;

        _laserCooldownTimer = laserCooldown;

        foreach (Weapon weapon in weapons)
        {
            weapon.Initialize(this, healthSystem);
        }
    }

    private void Dash()
    {
        if (Dashing || _dashCooldownTimer < dashCooldown || _movementDirection.magnitude == 0f) return;

        _dashDirection = _movementDirection;
        _dashOrigin = transform.position;
        Dashing = true;
    }

    private void FireLaser()
    {
        if (_laserCooldownTimer < laserCooldown) return;

        //Physics
        Ray2D ray = new(laserFirePoint.position, _mousePosition - (Vector2)laserFirePoint.position);
        float rayDistance = laserMaxDistance;
        //Checks obstacles
        RaycastHit2D obstacleHit = Physics2D.Raycast(ray.origin, ray.direction, rayDistance, obstacleLayer);
        if (obstacleHit.collider != null)
        {
            rayDistance = obstacleHit.distance;
        }
        //Creates the damaging ray
        RaycastHit2D[] hits = Physics2D.BoxCastAll(ray.origin, new Vector2(laserWidth, 1), mesh.rotation.eulerAngles.y, ray.direction, rayDistance, enemyLayer);
        foreach (RaycastHit2D enemyHit in hits)
        {
            if (enemyHit.collider.TryGetComponent(out HealthSystem healthSystem))
            {
                healthSystem.TakeDamage(laserDamage, transform);
            }
        }

        ////Visuals
        Laser newLaser = Instantiate(laserPrefab);
        newLaser.Initialize(laserFirePoint.position, (Vector2)laserFirePoint.position + (_mousePosition - (Vector2)laserFirePoint.position).normalized * rayDistance, laserWidth);

        _laserCooldownTimer = 0f;
    }

    private void TakeDamage(Transform damageSource)
    {
        if (Dashing)
        {
            healthSystem.PreventDamage = true;
        }
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
        HandleRotationInput();

        Rotate();

        if (_dashCooldownTimer < dashCooldown)
        {
            _dashCooldownTimer += Time.deltaTime;
        }

        if (_laserCooldownTimer < laserCooldown)
        {
            _laserCooldownTimer += Time.deltaTime;
        }
    }

    private void HandleMovementInput()
    {
        _movementDirection = _controls.InGame.Movement.ReadValue<Vector2>();
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
        foreach (Weapon weapon in weapons)
        {
            weapon.UpdatePhysics();
        }

        Move();
    }

    private void Move()
    {
        if (!Dashing)
        {
            Rb.velocity = movementSpeed * Time.fixedDeltaTime * _movementDirection;
        } else
        {
            Rb.velocity = dashSpeed * Time.fixedDeltaTime * _dashDirection;

            if (((Vector2)transform.position - _dashOrigin).magnitude >= dashDistance)
            {
                Dashing = false;
                _dashCooldownTimer = 0f;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            if (Dashing)
            {
                Dashing = false;
                _dashCooldownTimer = 0f;
            }
        }
    }
}
