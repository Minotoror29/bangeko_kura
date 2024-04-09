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

    [Header("Effects")]
    [SerializeField] private Scrap scrapPrefab;
    [SerializeField] private List<GameObject> explosionEffects;

    private EventInstance _deathSound;
    private EventInstance _damageSound;

    public EnemiesManager EnemiesManager { get { return _enemiesManager; } }
    public NewPlayerController Player { get { return _player; } }
    public float DistanceToPlayer { get { return _distanceToPlayer; } }
    public float PatrolTime { get { return patrolTime; } }
    public float MovementSpeed { get { return movementSpeed; } }
    public List<EnemyBehaviourData> Behaviours { get { return behaviours; } }

    public event Action<Transform> OnAllyDiedClose;

    public void Initialize(EnemiesManager enemiesManager, NewPlayerController player, GameManager gameManager)
    {
        base.Initialize(gameManager);

        _enemiesManager = enemiesManager;
        _player = player;

        HealthSystem.OnHit += TakeHit;
        HealthSystem.OnDeath += Die;

        foreach (Weapon weapon in weapons)
        {
            weapon.Initialize(this, HealthSystem);
        }

        _deathSound = RuntimeManager.CreateInstance("event:/Weapons/Enemy Explosion");
        _damageSound = RuntimeManager.CreateInstance("event:/Weapons/Enemy Hit");

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

    public override bool SwordAttack(float buildupTime)
    {
        return _currentState.CanAttackSword(buildupTime);
    }

    public override bool Dash(float dashTime, float dashSpeed, Vector2 dashDirection)
    {
        return _currentState.CanDash(dashTime, dashSpeed, dashDirection);
    }

    private void TakeHit(Transform source, Knockback knockback)
    {
        _damageSound.start();

        if (!_currentState.CanBeKnockedBack()) return;

        ChangeState(new EnemyKnockBackState(this, (Vector2)(transform.position - source.position).normalized, knockback));
    }

    public void Die(HealthSystem healthSystem, Transform deathSource)
    {
        Scrap newScrap = Instantiate(scrapPrefab, transform.position, Quaternion.identity);
        newScrap.Initialize();

        int randomExplosion = UnityEngine.Random.Range(0, explosionEffects.Count);
        float randomExplosionRotation = UnityEngine.Random.Range(0f, 360f);
        float randomExplosionScale = UnityEngine.Random.Range(0.75f, 1f);
        GameObject newExplosion = Instantiate(explosionEffects[randomExplosion], transform.position, Quaternion.Euler(0f, 0f, randomExplosionRotation));
        newExplosion.transform.localScale = new Vector3(randomExplosionScale, randomExplosionScale, 1f);
        Destroy(newExplosion, 0.367f);

        foreach (EnemyController enemy in EnemiesManager.EnemiesCloseTo(this, fleeingZoneRadius))
        {
            enemy.EnemyDiedClose(deathSource);
        }

        _enemiesManager.RemoveEnemy(this);

        _deathSound.start();

        gameObject.SetActive(false);
    }

    public void PlayerDied()
    {
        _currentState.PlayerDied();
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        _distanceToPlayer = (_player.transform.position - transform.position).magnitude;

        foreach (Weapon weapon in weapons)
        {
            weapon.UpdateLogic();
        }

        _currentState.UpdateLogic();
    }

    public void LookTowards(Vector2 direction, bool idle)
    {
        if (Rb.velocity.magnitude > 0 || idle)
        {
            Quaternion meshRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.y), Mesh.up);
            Mesh.localRotation = Quaternion.Euler(new Vector3(0f, meshRotation.eulerAngles.y, 0f));
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
        Rb.velocity = Vector3.zero;
    }

    public void MoveTowards(Vector2 direction, float speed)
    {
        if (!Dashing)
        {
            Rb.velocity = speed * Time.fixedDeltaTime * direction.normalized;
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

    private void OnCollisionStay2D(Collision2D collision)
    {
        _currentState.OnCollisionStay(collision);
    }

    private void OnDrawGizmos()
    {
        //Gizmos.color = Color.blue;
        //Gizmos.DrawWireSphere(transform.position, fleeingZoneRadius);
    }
}
