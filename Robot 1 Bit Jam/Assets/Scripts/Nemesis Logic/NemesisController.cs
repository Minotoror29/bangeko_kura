using FMOD.Studio;
using FMODUnity;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NemesisController : Controller
{
    private HealthSystem _healthSystem;
    private NewPlayerController _player;

    [SerializeField] private float movementSpeed = 1000f;

    [SerializeField] private List<Weapon> weapons;

    [SerializeField] private Transform mesh;

    private NemesisState _currentState;

    private EventInstance _deathSound;

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

        _healthSystem = GetComponent<HealthSystem>();
        _healthSystem.Initialize(transform);
        _healthSystem.OnDeath += Die;
        _player = FindObjectOfType<NewPlayerController>();

        foreach (Weapon weapon in weapons)
        {
            weapon.Initialize(this, _healthSystem);
        }

        _deathSound = RuntimeManager.CreateInstance("event:/Boss Death");

        ChangeState(new NemesisFarState(this, _player));
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

        _currentState.UpdateLogic();
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

        _currentState.UpdatePhysics();
    }

    public void MoveTowards(Vector3 direction)
    {
        Animator.SetBool("Walking", true);

        if (!Dashing)
        {
            Rb.velocity = movementSpeed * Time.fixedDeltaTime * direction.normalized;
        }
    }

    public void StopMovement()
    {
        Animator.SetBool("Walking", false);

        Rb.velocity = Vector2.zero;
    }
}
