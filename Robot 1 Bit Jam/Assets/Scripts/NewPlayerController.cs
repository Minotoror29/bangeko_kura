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

    [SerializeField] private Transform mesh;
    private Vector2 _lookDirection;

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
        _controls.InGame.Laser.performed += ctx => Laser();

        _dashCooldownTimer = dashCooldown;
    }

    private void Dash()
    {
        if (_dashing || _dashCooldownTimer < dashCooldown || _movementDirection.magnitude == 0f) return;

        _dashDirection = _movementDirection;
        _dashOrigin = transform.position;
        _dashing = true;
    }

    private void Laser()
    {

    }

    public void UpdateLogic()
    {
        HandleMovementInput();
        HandleRotationInput();

        if (_dashCooldownTimer < dashCooldown)
        {
            _dashCooldownTimer += Time.deltaTime;
        }
    }

    private void HandleMovementInput()
    {
        _movementDirection = _controls.InGame.Movement.ReadValue<Vector2>();
    }

    private void HandleRotationInput()
    {
        _lookDirection = Camera.main.ScreenToWorldPoint(_controls.InGame.MousePosition.ReadValue<Vector2>()) - transform.position;

        //mesh.LookAt(new Vector3(_lookDirection.x, 0f, _lookDirection.y));
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
