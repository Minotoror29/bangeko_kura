using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class ArenaManager : ScreenManager
{
    [SerializeField] private Transform spawnCursorPrefab;
    [SerializeField] private LayerMask groundMask;

    [Space]
    [SerializeField] private List<EnemiesManager> waves;
    [SerializeField] private float waveDelay;

    [Space]
    [SerializeField] private List<SwitchPlatform> switchPlatforms;

    private ScreenControls _controls;
    private Transform _spawnCursor;
    private Vector2 _landPosition;
    private GameObject _ground;
    private float _waveDelayTimer;

    private bool _started = false;

    private EnemiesManager _currentWave;

    public UnityEvent OnStartArena;
    public UnityEvent OnEndArena;

    public override void Initialize(GameManager gameManager, PlayerController player)
    {
        base.Initialize(gameManager, player);

        foreach (SwitchPlatform platform in switchPlatforms)
        {
            platform.Initialize(true);
        }
    }

    public override void DetermineSpawnPoint()
    {
        CurrentState = ScreenState.Spawn;
        _controls.Spawn.Enable();
        _spawnCursor = Instantiate(spawnCursorPrefab, DefaultSpawnPoint.position, Quaternion.identity);
    }

    public override void EnterScreen()
    {
        base.EnterScreen();

        _controls = new ScreenControls();
        _controls.Spawn.Spawn.performed += ctx => SpawnPlayer();
    }

    public override void ExitScreen(ScreenExit lastExit)
    {
        base.ExitScreen(lastExit);

        _controls.Spawn.Spawn.performed -= ctx => SpawnPlayer();
    }

    public void StartArena()
    {
        if (_started) return;

        OnStartArena?.Invoke();

        foreach (ScreenExit exit in Exits)
        {
            exit.gameObject.SetActive(false);
        }

        if (_currentWave == null)
        {
            _started = true;

            foreach (SwitchPlatform platform in switchPlatforms)
            {
                platform.Exit();
            }

            ChangeWave(waves[0]);
        }
    }

    private void ChangeWave(EnemiesManager nextWave)
    {
        if (_currentWave != null)
        {
            _currentWave.gameObject.SetActive(false);
        }

        _currentWave = nextWave;
        _waveDelayTimer = waveDelay;
    }

    private void CheckIfArenaEnds()
    {
        if (waves.IndexOf(_currentWave) == waves.Count - 1)
        {
            OnEndArena?.Invoke();

            _currentWave.gameObject.SetActive(false);

            foreach (ScreenExit exit in Exits)
            {
                exit.gameObject.SetActive(true);
            }

            foreach (SwitchPlatform platform in switchPlatforms)
            {
                platform.Enter();
            }
        } else
        {
            ChangeWave(waves[waves.IndexOf(_currentWave) + 1]);
        }
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        if (CurrentState == ScreenState.Spawn)
        {
            Ray ray = Camera.main.ScreenPointToRay(_controls.Spawn.MousePosition.ReadValue<Vector2>());
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, groundMask);
            if (hit)
            {
                _spawnCursor.position = hit.point;
                _landPosition = hit.point;
                _ground = hit.collider.gameObject;
            }
        }

        if (_waveDelayTimer > 0f)
        {
            _waveDelayTimer -= Time.deltaTime;

            if (_waveDelayTimer <= 0f)
            {
                _currentWave.gameObject.SetActive(true);
                _currentWave.Initialize(true);
                _currentWave.OnAllEnemiesDead += CheckIfArenaEnds;
            }
        }
    }

    private void SpawnPlayer()
    {
        CurrentState = ScreenState.Play;
        _controls.Spawn.Disable();
        Destroy(_spawnCursor.gameObject);

        Player.ChangeState(new PlayerLandState(Player, _landPosition, _ground));
    }
}
