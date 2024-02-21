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

    [Header("Idle")]
    [SerializeField] private float minIdleTime = 3f;
    [SerializeField] private float maxIdleTime = 7f;

    [Header("Patrol")]
    [SerializeField] private float patrolTime = 4f;
    [SerializeField] private float movementSpeed = 250f;

    [SerializeField] private List<EnemyBehaviourData> behaviours;
    [SerializeField] private float fleeingZoneRadius = 10f;

    [SerializeField] private List<Weapon> weapons;

    [SerializeField] private Transform mesh;

    private EventInstance _deathSound;

    public EnemiesManager EnemiesManager { get { return _enemiesManager; } }
    public NewPlayerController Player { get { return _player; } }
    public float DistanceToPlayer { get { return _distanceToPlayer; } }
    public float PatrolTime { get { return patrolTime; } }
    public List<EnemyBehaviourData> Behaviours { get { return behaviours; } }

    public event Action<Transform> OnAllyDiedClose;

    public void Initialize(EnemiesManager enemiesManager, NewPlayerController player, ScreenManager screenManager)
    {
        base.Initialize(screenManager);

        _enemiesManager = enemiesManager;
        _player = player;

        HealthSystem.OnDeath += Die;

        foreach (Weapon weapon in weapons)
        {
            weapon.Initialize(this, HealthSystem);
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

        gameObject.SetActive(false);
    }

    public override void UpdateLogic()
    {
        _distanceToPlayer = (_player.transform.position - transform.position).magnitude;

        foreach (Weapon weapon in weapons)
        {
            weapon.UpdateLogic();
        }

        _currentState.UpdateLogic();
    }

    public void LookTowards(Vector2 direction)
    {
        if (Rb.velocity.magnitude > 0)
        {
            Quaternion meshRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.y), mesh.up);
            mesh.localRotation = Quaternion.Euler(new Vector3(0f, meshRotation.eulerAngles.y, 0f));
        }
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
        if (deathSource == null) return;

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
