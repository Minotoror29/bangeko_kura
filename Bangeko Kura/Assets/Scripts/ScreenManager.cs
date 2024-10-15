using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ScreenState { Inactive, Play, Spawn }

public class ScreenManager : MonoBehaviour
{
    private ScreenState _currentState;

    private PlayerController _player;
    [SerializeField] private Transform defaultSpawnPoint;
    [SerializeField] private GameObject defaultSpawnGround;
    [SerializeField] private List<ScreenExit> exits;
    [SerializeField] private CinemachineVirtualCamera vCam;
    
    private ScreenExit _lastExit;

    public event Action<bool> OnInitialize;
    public event Action OnUpdate;
    public event Action OnFixedUpdate;
    public event Action OnPlayerDeath;
    public event Action<bool> OnEnter;
    public event Action<bool> OnExit;
    public event Action<PlayerController> OnChangePlayerController;
    public event Action OnPauseScreen;

    public ScreenState CurrentState { get { return _currentState; } set { _currentState = value; } }
    public PlayerController Player { get { return _player; } }
    public Transform DefaultSpawnPoint { get { return defaultSpawnPoint; } }
    public List<ScreenExit> Exits { get { return exits; } }

    public virtual void Initialize(GameManager gameManager, PlayerController player)
    {
        _currentState = ScreenState.Inactive;

        _player = player;

        foreach (ScreenExit exit in exits)
        {
            exit.Initialize(gameManager);
            exit.gameObject.SetActive(false);
        }

        vCam.gameObject.SetActive(false);

        OnInitialize?.Invoke(false);
    }

    public virtual void EnterScreen()
    {
        _currentState = ScreenState.Play;

        foreach (ScreenExit exit in exits)
        {
            exit.gameObject.SetActive(true);
        }

        CameraManager.Instance.ChangeCamera(vCam);

        OnEnter?.Invoke(true);
    }

    public virtual void ExitScreen(ScreenExit lastExit)
    {
        _lastExit = lastExit;

        _currentState = ScreenState.Inactive;

        foreach (ScreenExit exit in exits)
        {
            exit.gameObject.SetActive(false);
        }

        OnExit?.Invoke(false);
    }

    public void ChangePlayerController(PlayerController newPlayer)
    {
        _player = newPlayer;

        OnChangePlayerController?.Invoke(newPlayer);
    }

    public void PlayerFell()
    {
        OnPlayerDeath?.Invoke();

        DetermineSpawnPoint();
    }

    public virtual void DetermineSpawnPoint()
    {
        Vector2 spawnPosition = defaultSpawnPoint.position;
        GameObject spawnGround = defaultSpawnGround;

        if (_lastExit != null)
        {
            spawnPosition = _lastExit.RelativeSpawnPoint.position;
            spawnGround = _lastExit.RelativeSpawnGround;
        }

        _player.ChangeState(new PlayerLandState(_player, spawnPosition, spawnGround));
    }

    public void ChangeScreenCamera(CinemachineVirtualCamera newCam)
    {
        vCam.gameObject.SetActive(false);
        vCam = newCam;
        vCam.gameObject.SetActive(true);
    }

    public void PauseScreen()
    {
        Debug.Log("Pause Screen");
        OnPauseScreen?.Invoke();
    }

    public virtual void UpdateLogic()
    {
        OnUpdate?.Invoke();
    }    

    public void UpdatePhysics()
    {
        OnFixedUpdate?.Invoke();
    }
}
