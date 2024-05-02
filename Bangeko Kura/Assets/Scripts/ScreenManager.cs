using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ScreenState { Inactive, Play, Spawn }

public class ScreenManager : MonoBehaviour
{
    private ScreenState _currentState;

    [SerializeField] private PlayerController player;
    [SerializeField] private Transform defaultSpawnPoint;
    [SerializeField] private GameObject defaultSpawnGround;
    [SerializeField] private List<ScreenExit> exits;
    [SerializeField] private CinemachineVirtualCamera vCam;
    
    private ScreenExit _lastExit;

    public event Action OnInitialize;
    public event Action OnUpdate;
    public event Action OnFixedUpdate;
    public event Action OnPlayerDeath;
    public event Action<bool> OnEnter;
    public event Action<bool> OnExit;

    public ScreenState CurrentState { get { return _currentState; } set { _currentState = value; } }
    public PlayerController Player { get { return player; } }
    public Transform DefaultSpawnPoint { get { return defaultSpawnPoint; } }

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

        player.ChangeState(new PlayerLandState(player, spawnPosition, spawnGround));
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
