using FMOD.Studio;
using FMODUnity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : Controller
{
    private EnemiesManager _enemiesManager;

    private PlayerController _player;
    private float _distanceToPlayer;

    private EnemyState _currentState;

    [Header("Idle")]
    [SerializeField] private float minIdleTime = 3f;
    [SerializeField] private float maxIdleTime = 7f;

    [Header("Patrol")]
    [SerializeField] private float patrolTime = 4f;
    [SerializeField] private float movementSpeed = 250f;
    [SerializeField] private LayerMask voidLayer;

    [SerializeField] private List<EnemyBehaviourData> behaviours;
    [SerializeField] private float fleeingZoneRadius = 10f;

    [SerializeField] private List<Weapon> weapons;

    [Header("Effects")]
    [SerializeField] private Scrap scrapPrefab;
    [SerializeField] private List<GameObject> explosionEffects;

    [Header("Land")]
    [SerializeField] private GameObject landMeshPrefab;
    [SerializeField] private int landDamage = 3;
    [SerializeField] private float landDamageRadius = 2.25f;
    [SerializeField] private float landKnockbackDistance;
    [SerializeField] private float landKnockbackSpeed;
    [SerializeField] private GameObject shadowPrefab;
    [SerializeField] private GameObject startGround;
    private Knockback _landKnockback;
    private GameObject _landMesh;

    [Header("Fall")]
    [SerializeField] private GameObject fallSpritePrefab;
    private GameObject _fallSprite;
    private List<GameObject> _lastGrounds;

    [Space]
    [SerializeField] private bool spawnIdle = true;
    [SerializeField] private bool canBeKnockedback = true;

    private bool _damagedByPlayer = false;

    private EventInstance _deathSound;
    private EventInstance _damageSound;

    public EnemiesManager EnemiesManager { get { return _enemiesManager; } }
    public PlayerController Player { get { return _player; } }
    public float DistanceToPlayer { get { return _distanceToPlayer; } }
    public float PatrolTime { get { return patrolTime; } }
    public float MovementSpeed { get { return movementSpeed; } }
    public LayerMask VoidLayer { get { return voidLayer; } }
    public List<EnemyBehaviourData> Behaviours { get { return behaviours; } }
    public GameObject LandMesh { get { return _landMesh; } }
    public int LandDamage { get { return landDamage; } }
    public float LandDamageRadius { get { return landDamageRadius; } }
    public GameObject ShadowPrefab { get { return shadowPrefab; } }
    public Knockback LandKnockback { get { return _landKnockback; } }
    public GameObject FallSprite { get { return _fallSprite; } }
    public bool DamagedByPlayer { get { return _damagedByPlayer; } set { _damagedByPlayer = value; } }

    public event Action<Transform> OnAllyDiedClose;

    public void Initialize(EnemiesManager enemiesManager, PlayerController player, GameManager gameManager)
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

        _landKnockback = new Knockback { knockbackDistance = landKnockbackDistance, knockbackSpeed = landKnockbackSpeed };

        _fallSprite = Instantiate(fallSpritePrefab);
        _fallSprite.SetActive(false);

        _deathSound = RuntimeManager.CreateInstance("event:/Weapons/Enemy Explosion");
        _damageSound = RuntimeManager.CreateInstance("event:/Weapons/Enemy Hit");

        AddGround(startGround);

        if (spawnIdle)
        {
            ChangeState(new EnemyIdleState(this));
        } else
        {
            _landMesh = Instantiate(landMeshPrefab);
            _landMesh.SetActive(false);
            ChangeState(new EnemyLandState(this));
        }
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

        if (source == _player.transform)
        {
            _damagedByPlayer = true;
        }

        if (!_currentState.CanBeKnockedBack() || !canBeKnockedback) return;

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

    public void ChangePlayerController(PlayerController newPlayer)
    {
        _player = newPlayer;
    }

    public void PlayerDied()
    {
        _currentState.PlayerDied();
    }

    public override void SetCollidersActive(bool active)
    {
        base.SetCollidersActive(active);

        foreach (Weapon weapon in weapons)
        {
            if (weapon.TryGetComponent(out Collider2D coll))
            {
                coll.enabled = active;
            }
        }
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
        if (Rb.velocity != Vector2.zero || idle)
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

    public void Activate(bool activate)
    {
        if (!activate)
        {
            _lastGrounds = new();
            foreach (GameObject ground in Grounds)
            {
                _lastGrounds.Add(ground);
            }
        } else
        {
            if (_lastGrounds != null && _lastGrounds.Count > 0)
            {
                foreach (GameObject ground in _lastGrounds)
                {
                    Grounds.Add(ground);
                }
            }
        }
        ChangeState(new EnemyIdleState(this));
        gameObject.SetActive(activate);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        _currentState.OnCollisionEnter(collision);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        _currentState.OnCollisionStay(collision);
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

        _currentState.OnTriggerEnter(collision);
    }

    private void OnDrawGizmos()
    {
        //Gizmos.color = Color.blue;
        //Gizmos.DrawWireSphere(transform.position, fleeingZoneRadius);
    }
}
