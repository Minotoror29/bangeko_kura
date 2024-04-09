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
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private GameObject spawnGround;
    [SerializeField] private bool isArena = false;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private Transform spawnCursorPrefab;
    [SerializeField] private List<ScreenExit> exits;
    [SerializeField] private CinemachineVirtualCamera vCam;

    private Vector2 _spawnPosition;
    private Transform _spawnCursor;
    private GameObject _ground;

    public event Action OnPlayerDeath;

    public void Initialize(GameManager gameManager)
    {
        _currentState = ScreenState.Inactive;

        foreach (ScreenExit exit in exits)
        {
            exit.Initialize(gameManager);
            exit.gameObject.SetActive(false);
        }

        vCam.gameObject.SetActive(false);
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
    }

    public void ExitScreen()
    {
        _controls.Spawn.Spawn.performed -= ctx => SpawnPlayer();
        _controls.Spawn.Disable();
        _currentState = ScreenState.Inactive;

        foreach (ScreenExit exit in exits)
        {
            exit.gameObject.SetActive(true);
        }
    }

    public void PlayerFell()
    {
        OnPlayerDeath?.Invoke();

        if (!isArena)
        {
            player.ChangeState(new PlayerLandState(player, spawnPoint.position, spawnGround));
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
                _ground = hit.collider.gameObject;
            }

            //_spawnPosition = Camera.main.ScreenToWorldPoint(_controls.Spawn.MousePosition.ReadValue<Vector2>());
        }
    }

    private void SpawnPlayer()
    {
        _currentState = ScreenState.Play;
        _controls.Spawn.Disable();
        Destroy(_spawnCursor.gameObject);

        player.ChangeState(new PlayerLandState(player, _spawnPosition, _ground));
    }
}
