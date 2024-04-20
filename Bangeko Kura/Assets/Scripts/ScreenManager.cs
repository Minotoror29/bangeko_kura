using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ScreenState { Inactive, Play, Spawn }

public class ScreenManager : MonoBehaviour
{
    private ScreenControls _controls;
    private ScreenState _currentState;

    [SerializeField] private NewPlayerController player;
    [SerializeField] private Transform defaultSpawnPoint;
    [SerializeField] private GameObject defaultSpawnGround;
    [SerializeField] private bool isArena = false;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private Transform spawnCursorPrefab;
    [SerializeField] private List<ScreenExit> exits;
    [SerializeField] private CinemachineVirtualCamera vCam;

    private Vector2 _landPosition;
    private Transform _spawnCursor;
    private GameObject _ground;

    private ScreenExit _lastExit;

    public event Action OnInitialize;
    public event Action OnUpdate;
    public event Action OnFixedUpdate;
    public event Action OnPlayerDeath;
    public event Action<bool> OnEnter;
    public event Action<bool> OnExit;

    public void Initialize(GameManager gameManager)
    {
        _currentState = ScreenState.Inactive;

        foreach (ScreenExit exit in exits)
        {
            exit.Initialize(gameManager);
            exit.gameObject.SetActive(false);
        }

        vCam.gameObject.SetActive(false);

        OnInitialize?.Invoke();
    }

    public void EnterScreen()
    {
        _controls = new ScreenControls();
        _controls.Spawn.Spawn.performed += ctx => SpawnPlayer();
        _currentState = ScreenState.Play;

        foreach (ScreenExit exit in exits)
        {
            exit.gameObject.SetActive(true);
        }

        CameraManager.Instance.ChangeCamera(vCam);

        OnEnter?.Invoke(true);
    }

    public void ExitScreen(ScreenExit lastExit)
    {
        _lastExit = lastExit;

        _controls.Spawn.Spawn.performed -= ctx => SpawnPlayer();
        _controls.Spawn.Disable();
        _currentState = ScreenState.Inactive;

        foreach (ScreenExit exit in exits)
        {
            exit.gameObject.SetActive(false);
        }

        OnExit?.Invoke(false);
    }

    public void PlayerFell()
    {
        OnPlayerDeath?.Invoke();

        if (!isArena)
        {
            Vector2 spawnPosition = defaultSpawnPoint.position;
            GameObject spawnGround = defaultSpawnGround;

            if (_lastExit != null)
            {
                spawnPosition = _lastExit.RelativeSpawnPoint.position;
                spawnGround = _lastExit.RelativeSpawnGround;
            }

            player.ChangeState(new PlayerLandState(player, spawnPosition, spawnGround));
        } else
        {
            _currentState = ScreenState.Spawn;
            _controls.Spawn.Enable();
            _spawnCursor = Instantiate(spawnCursorPrefab, defaultSpawnPoint.position, Quaternion.identity);
        }
    }

    public void UpdateLogic()
    {
        if (_currentState == ScreenState.Spawn)
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

        OnUpdate?.Invoke();
    }

    private void SpawnPlayer()
    {
        _currentState = ScreenState.Play;
        _controls.Spawn.Disable();
        Destroy(_spawnCursor.gameObject);

        player.ChangeState(new PlayerLandState(player, _landPosition, _ground));
    }

    public void UpdatePhysics()
    {
        OnFixedUpdate?.Invoke();
    }
}
