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
    [SerializeField] private int laserDamage = 15;

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
    }

    public void UpdateLogic()
    {
        HandleMovementInput();
        HandleLookInput();

        Look();
    }

    private void HandleMovementInput()
    {
        _movementDirection.x = _playerControls.InGame.Movement.ReadValue<Vector2>().x;
        _movementDirection.z = _playerControls.InGame.Movement.ReadValue<Vector2>().y;
    }

    private void HandleLookInput()
    {
        Ray ray = Camera.main.ScreenPointToRay(_playerControls.InGame.MousePosition.ReadValue<Vector2>());
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
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
        Laser newLaser = Instantiate(laserPrefab);
        newLaser.Initialize(laserFirePoint.position, laserFirePoint.position + transform.forward * laserMaxDistance);

        Ray ray = new Ray(laserFirePoint.position, transform.forward);
        RaycastHit[] hits = Physics.RaycastAll(ray, laserMaxDistance, enemyLayer);
        foreach (RaycastHit hit in hits)
        {
            hit.collider.GetComponent<HealthSystem>().TakeDamage(laserDamage);
        }
    }
}
