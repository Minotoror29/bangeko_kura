using FMOD.Studio;
using FMODUnity;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NemesisController : Controller
{
    private NewPlayerController _player;

    [SerializeField] private ShieldController shield;

    [SerializeField] private Transform mesh;

    [Header("Phases")]
    [SerializeField] private NemesisPhaseData phase1;
    [SerializeField, Range(0, 100), Tooltip("In percentage")] private int healthToPhase2 = 75;
    [SerializeField] private NemesisPhaseData phase2;
    private NemesisPhase _currentPhase;

    [Header("Sword")]
    [SerializeField] private float swordDistance = 1f;
    [SerializeField] private float swordChargeTime = 1f;
    [SerializeField] private float swordCooldown = 3f;
    [SerializeField] private int swordDamage = 10;
    private float _swordCooldownTimer;

    [Header("Walk")]
    [SerializeField] private float walkDistance = 2f;
    [SerializeField] private float walkSpeed = 250f;

    [Header("Shoot")]
    [SerializeField] private float shootDistance = 3f;
    [SerializeField] private BulletController bulletPrefab;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private float shootMaxRandomAngle = 5f;

    [Header("Dash")]
    [SerializeField] private float dashDistance = 5f;
    [SerializeField] private float dashSpeed = 500f;

    [Header("Stun")]
    [SerializeField] private float stunTime = 5f;

    private EventInstance _deathSound;

    public NewPlayerController Player { get { return _player; } }
    public float SwordDistance { get { return swordDistance; } }
    public float SwordChargeTime { get { return swordChargeTime; } }
    public float SwordCooldown { get { return swordCooldown; } }
    public float SwordCooldownTimer { get { return _swordCooldownTimer; } set { _swordCooldownTimer = value; } }
    public int SwordDamage { get { return swordDamage; } }
    public float WalkDistance { get { return walkDistance; } }
    public float WalkSpeed { get { return walkSpeed; } }
    public float ShootDistance { get { return shootDistance; } }
    public float DashDistance { get { return dashDistance; } }
    public float DashSpeed { get { return dashSpeed; } }
    public float StunTime { get { return stunTime; } }

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

    public override void Initialize()
    {
        base.Initialize();

        HealthSystem.OnDamage += TakeDamage;
        HealthSystem.OnDeath += Die;
        _player = FindObjectOfType<NewPlayerController>();

        shield.Initialize(this, HealthSystem);

        _deathSound = RuntimeManager.CreateInstance("event:/Boss Death");

        ChangePhase(phase1.Phase(this));
    }

    public void ChangePhase(NemesisPhase nextPhase)
    {
        _currentPhase?.Exit();
        _currentPhase = nextPhase;
        _currentPhase.Enter();
    }

    public void CheckHealth()
    {
        if (HealthSystem.HealthRatio <= healthToPhase2)
        {
            ChangePhase(phase2.Phase(this));
            shield.gameObject.SetActive(false);
        }
    }

    public void ShootBullet()
    {
        float randomAngle = Random.Range(-shootMaxRandomAngle, shootMaxRandomAngle);
        Quaternion addedRotation = Quaternion.AngleAxis(randomAngle, Vector3.back);
        Vector3 rotatedDirection = addedRotation * (_player.transform.position - shootPoint.position);

        BulletController newBullet = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);
        newBullet.Initialize(rotatedDirection, transform);
    }

    private void TakeDamage()
    {
        _currentPhase.TakeDamage();
    }

    private void Die(HealthSystem healthSystem, Transform deathSource)
    {
        _deathSound.start();

        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public override void UpdateLogic()
    {
        shield.UpdateLogic();

        Rotate();

        _currentPhase?.UpdateLogic();

        if (_swordCooldownTimer > 0f)
        {
            _swordCooldownTimer -= Time.deltaTime;
        }
    }

    private void Rotate()
    {
        Vector2 lookDirection = _player.transform.position - transform.position;
        Quaternion meshRotation = Quaternion.LookRotation(new Vector3(lookDirection.x, 0f, lookDirection.y), mesh.up);
        mesh.localRotation = Quaternion.Euler(new Vector3(0f, meshRotation.eulerAngles.y, 0f));
    }

    public override void UpdatePhysics()
    {
        _currentPhase?.UpdatePhysics();
    }

    public void MoveTowards(Vector3 direction, float speed)
    {
         Rb.velocity = speed * Time.fixedDeltaTime * direction.normalized;
    }

    public void StopMovement()
    {
        Animator.SetBool("Walking", false);

        Rb.velocity = Vector2.zero;
    }

    private void OnDrawGizmos()
    {
        //Behaviour Zones
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, swordDistance);
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, walkDistance);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, shootDistance);

        //Shooting Angles
        Vector2 direction;
        if (_player == null)
        {
            direction = (((Vector2)shootPoint.position + Vector2.up - (Vector2)shootPoint.position) * 10f);
        } else
        {
            direction = _player.transform.position - shootPoint.position;
        }
        Gizmos.color = Color.yellow;
        Quaternion addedRotation = Quaternion.AngleAxis(shootMaxRandomAngle, Vector3.back);
        Vector3 rotatedDirection = addedRotation * direction;
        Gizmos.DrawRay(shootPoint.position, rotatedDirection);
        addedRotation = Quaternion.AngleAxis(-shootMaxRandomAngle, Vector3.back);
        rotatedDirection = addedRotation * direction;
        Gizmos.DrawRay(shootPoint.position, rotatedDirection);
    }
}
