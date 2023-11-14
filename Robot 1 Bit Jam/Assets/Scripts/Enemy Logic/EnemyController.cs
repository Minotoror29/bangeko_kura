using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyController : MonoBehaviour
{
    private EnemiesManager _enemiesManager;

    private EnemyState _currentState;

    private Rigidbody _rb;
    private HealthSystem _healthSystem;

    [Header("Idle")]
    [SerializeField] private float minIdleTime = 3f;
    [SerializeField] private float maxIdleTime = 7f;

    [Header("Patrol")]
    [SerializeField] private float patrolTime = 4f;
    [SerializeField] private float movementSpeed = 250f;

    public EnemiesManager EnemiesManager { get { return _enemiesManager; } }
    public float PatrolTime { get { return patrolTime; } }

    public event Action<EnemyController, Transform> OnDeath;

    public virtual void Initialize(EnemiesManager enemiesManager)
    {
        _enemiesManager = enemiesManager;

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

    public virtual void Die(Transform deathSource)
    {
        OnDeath?.Invoke(this, deathSource);
        _enemiesManager.RemoveEnemy(this);
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

    public abstract void EnemyDiedClose(Transform source);

    private void OnCollisionEnter(Collision collision)
    {
        _currentState.OnCollisionEnter(collision);
    }
}
