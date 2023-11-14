using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyController : MonoBehaviour
{
    private EnemiesManager _enemiesManager;

    private PlayerController _player;
    private float _distanceToPlayer;

    private EnemyState _currentState;

    private Rigidbody _rb;
    private HealthSystem _healthSystem;

    [Header("Idle")]
    [SerializeField] private float minIdleTime = 3f;
    [SerializeField] private float maxIdleTime = 7f;

    [Header("Patrol")]
    [SerializeField] private float patrolTime = 4f;
    [SerializeField] private float movementSpeed = 250f;

    [SerializeField] private List<EnemyBehaviourData> behaviours;
    [SerializeField] private float fleeingZoneRadius = 10f;

    [SerializeField] private List<Weapon> weapons;

    public EnemiesManager EnemiesManager { get { return _enemiesManager; } }
    public PlayerController Player { get { return _player; } }
    public float DistanceToPlayer { get { return _distanceToPlayer; } }
    public float PatrolTime { get { return patrolTime; } }
    public List<EnemyBehaviourData> Behaviours { get { return behaviours; } }

    public event Action<EnemyController, Transform> OnDeath;

    public virtual void Initialize(EnemiesManager enemiesManager, PlayerController player)
    {
        _enemiesManager = enemiesManager;
        _player = player;

        _rb = GetComponent<Rigidbody>();
        _healthSystem = GetComponent<HealthSystem>();
        _healthSystem.Initialize();
        _healthSystem.OnDeath += Die;

        foreach (Weapon weapon in weapons)
        {
            weapon.Initialize(transform);
        }

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

        CreateFleeingZone(deathSource);
    }

    private void CreateFleeingZone(Transform deathSource)
    {
        foreach (EnemyController enemy in EnemiesManager.EnemiesCloseTo(this, fleeingZoneRadius))
        {
            enemy.EnemyDiedClose(deathSource);
        }
    }

    public virtual void UpdateLogic()
    {
        _distanceToPlayer = (_player.transform.position - transform.position).magnitude;

        foreach (Weapon weapon in weapons)
        {
            weapon.UpdateLogic();
        }

        _currentState.UpdateLogic();

        transform.LookAt(_rb.velocity.normalized + transform.position);
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

    public virtual void EnemyDiedClose(Transform source) { }

    private void OnCollisionEnter(Collision collision)
    {
        _currentState.OnCollisionEnter(collision);
    }

    private void OnDrawGizmos()
    {
        //Gizmos.color = Color.blue;
        //Gizmos.DrawWireSphere(transform.position, fleeingZoneRadius);
    }
}
