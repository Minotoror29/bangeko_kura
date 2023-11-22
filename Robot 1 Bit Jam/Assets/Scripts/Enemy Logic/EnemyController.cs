using FMOD.Studio;
using FMODUnity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : Controller
{
    private EnemiesManager _enemiesManager;

    private NewPlayerController _player;
    private float _distanceToPlayer;

    private EnemyState _currentState;

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

    private EventInstance _deathSound;

    public EnemiesManager EnemiesManager { get { return _enemiesManager; } }
    public NewPlayerController Player { get { return _player; } }
    public float DistanceToPlayer { get { return _distanceToPlayer; } }
    public float PatrolTime { get { return patrolTime; } }
    public List<EnemyBehaviourData> Behaviours { get { return behaviours; } }

    public event Action<Transform> OnAllyDiedClose;

    public virtual void Initialize(EnemiesManager enemiesManager, NewPlayerController player)
    {
        base.Initialize();

        _enemiesManager = enemiesManager;
        _player = player;

        _healthSystem = GetComponent<HealthSystem>();
        _healthSystem.Initialize();
        _healthSystem.OnDeath += Die;

        foreach (Weapon weapon in weapons)
        {
            weapon.Initialize(this, _healthSystem);
        }

        _deathSound = RuntimeManager.CreateInstance("event:/Enemy Death");

        ChangeState(new EnemyIdleState(this));
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

    public void Die(HealthSystem healthSystem, Transform deathSource)
    {
        foreach (EnemyController enemy in EnemiesManager.EnemiesCloseTo(this, fleeingZoneRadius))
        {
            enemy.EnemyDiedClose(deathSource);
        }

        _enemiesManager.RemoveEnemy(this);

        _deathSound.start();

        Destroy(gameObject);
    }

    public override void UpdateLogic()
    {
        _distanceToPlayer = (_player.transform.position - transform.position).magnitude;

        foreach (Weapon weapon in weapons)
        {
            weapon.UpdateLogic();
        }

        _currentState.UpdateLogic();

        //transform.LookAt(Rb.velocity.normalized + (Vector2)transform.position);
    }

    public override void UpdatePhysics()
    {
        foreach (Weapon weapon in weapons)
        {
            weapon.UpdatePhysics();
        }

        _currentState.UpdatePhysics();
    }

    public void StopMovement()
    {
        if (Animator != null)
        {
            Animator.SetBool("Walking", false);
        }

        Rb.velocity = Vector3.zero;
    }

    public void MoveTowards(Vector2 direction)
    {
        if (Animator != null)
        {
            Animator.SetBool("Walking", true);
        }

        if (!Dashing)
        {
            Rb.velocity = movementSpeed * Time.fixedDeltaTime * direction.normalized;
        }
    }

    public virtual void EnemyDiedClose(Transform deathSource)
    {
        OnAllyDiedClose?.Invoke(deathSource);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        _currentState.OnCollisionEnter(collision);
    }

    private void OnDrawGizmos()
    {
        //Gizmos.color = Color.blue;
        //Gizmos.DrawWireSphere(transform.position, fleeingZoneRadius);
    }
}
