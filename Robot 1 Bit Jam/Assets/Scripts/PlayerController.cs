using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerControls _playerControls;
    private Rigidbody2D _rb;

    [SerializeField] private float movementSpeed = 100f;
    private Vector2 _movementDirection;

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

        _rb = GetComponent<Rigidbody2D>();
    }

    public void UpdateLogic()
    {
        HandleMovementInput();
    }

    private void HandleMovementInput()
    {
        _movementDirection = _playerControls.InGame.Movement.ReadValue<Vector2>();
    }

    public void UpdatePhysics()
    {
        _rb.velocity = movementSpeed * Time.fixedDeltaTime * _movementDirection;
    }
}
