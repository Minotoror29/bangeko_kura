using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerControls _playerControls;
    private Rigidbody _rb;

    [SerializeField] private float movementSpeed = 100f;
    private Vector3 _movementDirection;

    [SerializeField] LayerMask groundLayer;
    private Vector3 _lookDirection;

    [SerializeField] private Transform laserFirePoint;
    [SerializeField] private float laserMaxDistance = 10f;
    [SerializeField] private Laser laserPrefab;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private int laserDamage = 15;
    [SerializeField] private float laserCooldown = 3f;
    private float _laserTimer;

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
        _playerControls = new PlayerControls();
        _playerControls.InGame.Enable();
        _playerControls.InGame.Laser.performed += ctx => FireLaser();

        _rb = GetComponent<Rigidbody>();

        _laserTimer = laserCooldown;
    }

    public void UpdateLogic()
    {
        HandleMovementInput();
        HandleLookInput();

        Look();

        if (_laserTimer < laserCooldown)
        {
            _laserTimer += Time.deltaTime;
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
            _lookDirection = hit.point;

            Debug.DrawRay(Camera.main.transform.position, hit.point - Camera.main.transform.position, Color.red);
        }
    }

    private void Look()
    {
        transform.LookAt(_lookDirection);
    }

    public void UpdatePhysics()
    {
        _rb.velocity = movementSpeed * Time.fixedDeltaTime * _movementDirection;
    }

    private void FireLaser()
    {
        if (_laserTimer < laserCooldown) return;

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
            if (hit.collider.TryGetComponent<HealthSystem>(out var healthSystem))
            {
                healthSystem.TakeDamage(laserDamage);
            }
        }

        //Visuals
        Laser newLaser = Instantiate(laserPrefab);
        newLaser.Initialize(laserFirePoint.position, laserFirePoint.position + transform.forward * rayDistance);

        _laserTimer = 0f;
    }
}
