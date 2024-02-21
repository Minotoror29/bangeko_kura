using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ScreenState { Play, Spawn }

public class ScreenManager : MonoBehaviour
{
    private ScreenControls _controls;
    private ScreenState _currentState;

    [SerializeField] private NewPlayerController player;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private bool isArena = false;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private Transform spawnCursorPrefab;

    private Vector2 _spawnPosition;
    private Transform _spawnCursor;

    private void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        _controls = new ScreenControls();
        _controls.Spawn.Spawn.performed += ctx => SpawnPlayer();
        _currentState = ScreenState.Play;

        player.Initialize(this);
        player.ChangeState(new PlayerSpawnState(player, spawnPoint.position));
    }

    public void PlayerDied()
    {
        if (!isArena)
        {
            player.ChangeState(new PlayerSpawnState(player, spawnPoint.position));
        } else
        {
            _currentState = ScreenState.Spawn;
            _controls.Spawn.Enable();
            _spawnCursor = Instantiate(spawnCursorPrefab, spawnPoint.position, Quaternion.identity);
        }
    }

    private void Update()
    {
        if (_currentState == ScreenState.Spawn)
        {
            Ray ray = Camera.main.ScreenPointToRay(_controls.Spawn.MousePosition.ReadValue<Vector2>());
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, groundMask);
            if (hit)
            {
                _spawnCursor.position = hit.point;
                _spawnPosition = hit.point;
            }

            //_spawnPosition = Camera.main.ScreenToWorldPoint(_controls.Spawn.MousePosition.ReadValue<Vector2>());
        }
    }

    private void SpawnPlayer()
    {
        _currentState = ScreenState.Play;
        _controls.Spawn.Disable();
        Destroy(_spawnCursor.gameObject);

        player.ChangeState(new PlayerSpawnState(player, _spawnPosition));
    }
}
