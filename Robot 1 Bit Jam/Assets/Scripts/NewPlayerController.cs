using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class NewPlayerController : MonoBehaviour
{
    private Rigidbody2D _rb;
    private PlayerControls _controls;

    [Header("Movement")]
    [SerializeField] private float movementSpeed = 500f;
    private Vector2 _movementDirection;

    [Header("Dash")]
    [SerializeField] private float dashSpeed = 1000f;
    [SerializeField] private float dashDistance = 5f;
    [SerializeField] private float dashCooldown = 2f;
    private float _dashCooldownTimer;
    private Vector3 _dashDirection;
    private bool _dashing = false;
    private Vector2 _dashOrigin;

    [Header("Laser")]
    [SerializeField] private Transform laserFirePoint;
    [SerializeField] private float laserMaxDistance = 10f;
    [SerializeField] private Laser laserPrefab;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private int laserDamage = 15;
    [SerializeField] private float laserCooldown = 3f;
    [SerializeField] private Transform mesh;
    private float _laserCooldownTimer;
    private Vector2 _lookDirection;
    private Vector2 _mousePosition;

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

    public void Initialize()
    {
        _rb = GetComponent<Rigidbody2D>();

        _controls = new PlayerControls();
        _controls.InGame.Enable();
        _controls.InGame.Dash.performed += ctx => Dash();
        _controls.InGame.Laser.performed += ctx => FireLaser();

        _dashCooldownTimer = dashCooldown;

        _laserCooldownTimer = laserCooldown;
    }

    private void Dash()
    {
        if (_dashing || _dashCooldownTimer < dashCooldown || _movementDirection.magnitude == 0f) return;

        _dashDirection = _movementDirection;
        _dashOrigin = transform.position;
        _dashing = true;
    }

    private void FireLaser()
    {
        if (_laserCooldownTimer < laserCooldown) return;

        //Physics
        Ray ray = new(laserFirePoint.position, transform.forward);
        float rayDistance = laserMaxDistance;
        //if (Physics.Raycast(ray, out RaycastHit obstacleHit, laserMaxDistance, obstacleLayer))
        //{
        //    rayDistance = obstacleHit.distance;
        //}
        //RaycastHit[] hits = Physics.RaycastAll(ray, rayDistance, enemyLayer);
        //foreach (RaycastHit hit in hits)
        //{
        //    if (hit.collider.TryGetComponent(out HealthSystem healthSystem))
        //    {
        //        healthSystem.TakeDamage(laserDamage, transform);
        //    }
        //}

        ////Visuals
        Laser newLaser = Instantiate(laserPrefab);
        newLaser.Initialize(laserFirePoint.position, (Vector2)laserFirePoint.position + (_mousePosition - (Vector2)laserFirePoint.position).normalized * laserMaxDistance);

        _laserCooldownTimer = 0f;
    }

    public void UpdateLogic()
    {
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

    public void UpdatePhysics()
    {
        Move();
    }

    private void Move()
    {
        if (!_dashing)
        {
            _rb.velocity = movementSpeed * Time.fixedDeltaTime * _movementDirection;
        } else
        {
            _rb.velocity = dashSpeed * Time.fixedDeltaTime * _dashDirection;

            if (((Vector2)transform.position - _dashOrigin).magnitude >= dashDistance)
            {
                _dashing = false;
                _dashCooldownTimer = 0f;
            }
        }
    }
}
