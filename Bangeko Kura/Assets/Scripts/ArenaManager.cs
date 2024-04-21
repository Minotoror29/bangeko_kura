using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ArenaManager : ScreenManager
{
    [SerializeField] private Transform spawnCursorPrefab;
    [SerializeField] private LayerMask groundMask;

    [Space]
    [SerializeField] private List<EnemiesManager> waves;
    [SerializeField] private float waveDelay;

    private ScreenControls _controls;
    private Transform _spawnCursor;
    private Vector2 _landPosition;
    private GameObject _ground;

    private EnemiesManager _currentWave;

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

        if (_currentWave == null)
        {
            ChangeWave(waves[0]);
        }
    }

    public override void ExitScreen(ScreenExit lastExit)
    {
        base.ExitScreen(lastExit);

        _controls.Spawn.Spawn.performed -= ctx => SpawnPlayer();
    }

    private void ChangeWave(EnemiesManager nextWave)
    {
        if (_currentWave != null)
        {
            _currentWave.gameObject.SetActive(false);
        }

        _currentWave = nextWave;
        _currentWave.gameObject.SetActive(true);
        _currentWave.Initialize();
        _currentWave.OnAllEnemiesDead += CheckIfArenaEnds;
    }

    private void CheckIfArenaEnds()
    {
        if (waves.IndexOf(_currentWave) == waves.Count - 1)
        {
            _currentWave.gameObject.SetActive(false);
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
    }

    private void SpawnPlayer()
    {
        CurrentState = ScreenState.Play;
        _controls.Spawn.Disable();
        Destroy(_spawnCursor.gameObject);

        Player.ChangeState(new PlayerLandState(Player, _landPosition, _ground));
    }
}
