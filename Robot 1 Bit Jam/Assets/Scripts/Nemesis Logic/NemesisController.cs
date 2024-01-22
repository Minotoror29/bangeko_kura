using FMOD.Studio;
using FMODUnity;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NemesisController : Controller
{
    private NewPlayerController _player;

    [SerializeField] private List<Weapon> weapons;

    [SerializeField] private Transform mesh;

    private NemesisState _currentState;

    [Header("Sword")]
    [SerializeField] private float swordDistance = 1f;
    [SerializeField] private float swordChargeTime = 1f;
    [SerializeField] private float swordCooldown = 3f;
    private float _swordCooldownTimer;

    [Header("Walk")]
    [SerializeField] private float walkDistance = 2f;
    [SerializeField] private float walkSpeed = 250f;

    [SerializeField] private float shootDistance = 3f;

    [Header("Dash")]
    [SerializeField] private float dashDistance = 5f;
    [SerializeField] private float dashSpeed = 500f;

    private EventInstance _deathSound;

    public NewPlayerController Player { get { return _player; } }
    public float SwordDistance { get { return swordDistance; } }
    public float SwordChargeTime { get { return swordChargeTime; } }
    public float SwordCooldown { get { return swordCooldown; } }
    public float SwordCooldownTimer { get { return _swordCooldownTimer; } set { _swordCooldownTimer = value; } }
    public float WalkDistance { get { return walkDistance; } }
    public float WalkSpeed { get { return walkSpeed; } }
    public float ShootDistance { get { return shootDistance; } }
    public float DashDistance { get { return dashDistance; } }
    public float DashSpeed { get { return dashSpeed; } }

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

        HealthSystem.OnDeath += Die;
        _player = FindObjectOfType<NewPlayerController>();

        foreach (Weapon weapon in weapons)
        {
            weapon.Initialize(this, HealthSystem);
        }

        _deathSound = RuntimeManager.CreateInstance("event:/Boss Death");

        ChangeState(new NemesisIdleState(this));
    }

    public void ChangeState(NemesisState nextState)
    {
        _currentState?.Exit();
        _currentState = nextState;
        _currentState.Enter();
    }

    private void Die(HealthSystem healthSystem, Transform deathSource)
    {
        _deathSound.start();

        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public override void UpdateLogic()
    {
        foreach (Weapon weapon in weapons)
        {
            weapon.UpdateLogic();
        }

        Rotate();

        _currentState?.UpdateLogic();

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
        foreach (Weapon weapon in weapons)
        {
            weapon.UpdatePhysics();
        }

        _currentState?.UpdatePhysics();
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
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, swordDistance);
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, walkDistance);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, shootDistance);
    }
}
