using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private EnemyState _currentState;

    private Rigidbody _rb;
    private HealthSystem _healthSystem;

    [Header("Idle")]
    [SerializeField] private float minIdleTime = 3f;
    [SerializeField] private float maxIdleTime = 7f;

    [Header("Patrol")]
    [SerializeField] private float patrolRadius = 5f;
    [SerializeField] private float movementSpeed = 250f;

    public float PatrolRadius { get { return patrolRadius; } }

    public event Action<EnemyController> OnDeath;

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
        _rb = GetComponent<Rigidbody>();
        _healthSystem = GetComponent<HealthSystem>();
        _healthSystem.Initialize();
        _healthSystem.OnDeath += Die;

        ChangeState(new EnemyIdleState(this, GetRandomIdleTime()));
    }

    public void ChangeState(EnemyState nextState)
    {
        _currentState?.Exit();
        _currentState = nextState;
        _currentState.Enter();
    }

    public float GetRandomIdleTime()
    {
        return UnityEngine.Random.Range(minIdleTime, maxIdleTime);
    }

    private void Die()
    {
        OnDeath?.Invoke(this);
        Destroy(gameObject);
    }

    public void UpdateLogic()
    {
        _currentState.UpdateLogic();
    }

    public void UpdatePhysics()
    {
        _currentState.UpdatePhysics();
    }

    public void StopMovement()
    {
        _rb.velocity = Vector3.zero;
    }

    public void MoveTowards(Vector3 direction)
    {
        _rb.velocity = movementSpeed * Time.fixedDeltaTime * direction.normalized;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, patrolRadius);
    }
}
