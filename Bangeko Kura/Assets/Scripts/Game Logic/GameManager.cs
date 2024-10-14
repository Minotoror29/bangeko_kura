using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum StartingPlayerState { Idle, Elevator, Falling }

public class GameManager : MonoBehaviour
{
    private GameState _currentState;

    [SerializeField] private PlayerController player;

    [SerializeField] private StartingPlayerState startState;
    [SerializeField] private Elevator startElevator;
    [SerializeField] private ScreenManager startScreen;
    [SerializeField] private Transform startSpawnPoint;
    [SerializeField] private GameObject startGround;
    private ScreenManager _currentScreen;

    [SerializeField] private CameraManager cameraManager;
    [SerializeField] private CutsceneManager cutsceneManager;
    [SerializeField] private InGameCutsceneManager inGameCutsceneManager;
    [SerializeField] private Transform newPlayerSpawnPoint;

    [SerializeField] private Canvas gameCanvas;
    [SerializeField] private HealthDisplay healthDisplay;

    public event Action OnInitialize;

    public PlayerController Player { get { return player; } }
    public CutsceneManager CutsceneManager { get { return cutsceneManager; } }
    public InGameCutsceneManager InGameCutsceneManager { get { return inGameCutsceneManager; } }
    public Canvas GameCanvas { get { return gameCanvas; } }
    public HealthDisplay HealthDisplay { get {  return healthDisplay; } }

    private void Start()
    {
        player.Initialize(this);

        foreach (ScreenManager screen in FindObjectsOfType<ScreenManager>(true))
        {
            screen.Initialize(this, player);
        }

        if (cutsceneManager != null)
        {
            cutsceneManager.Initialize();
        }
        OnInitialize?.Invoke();

        ChangeScreen(startScreen, null);

        switch (startState)
        {
            case StartingPlayerState.Idle:
                player.ChangeState(new PlayerSpawnState(player, startSpawnPoint.position));
                break;
            case StartingPlayerState.Elevator:
                player.ChangeState(new PlayerWaitElevatorState(player, startElevator));
                break;
            case StartingPlayerState.Falling:
                player.ChangeState(new PlayerLandState(player, startSpawnPoint.position, startGround));
                break;
        }

        ChangeState(new GamePlayState(this));
    }

    public void ChangeState(GameState nextState)
    {
        _currentState?.Exit();
        _currentState = nextState;
        _currentState.Enter();
    }

    public void ChangeToCutsceneState()
    {
        _currentState?.Exit();
        _currentState = new GameCutsceneState(this);
        _currentState.Enter();
    }

    public void ChangeToPlayState()
    {
        _currentState?.Exit();
        _currentState = new GamePlayState(this);
        _currentState.Enter();
    }

    public void ChangeScreen(ScreenManager nextScreen, ScreenExit exit)
    {
        _currentScreen?.ExitScreen(exit);
        _currentScreen = nextScreen;
        _currentScreen.EnterScreen();

        ChangeState(new GameTransitionState(this));
    }

    public void ChangePlayerController(PlayerController newPlayerPrefab)
    {
        player.UnsubscribeEvents();
        Destroy(player.gameObject);

        PlayerController newPlayer = Instantiate(newPlayerPrefab, player.transform.position, Quaternion.identity);
        newPlayer.Initialize(this);
        player = newPlayer;
        player.ChangeState(new PlayerSpawnState(player, newPlayerSpawnPoint.position));

        foreach (ScreenManager screen in FindObjectsOfType<ScreenManager>(true))
        {
            screen.ChangePlayerController(newPlayer);
        }

        cameraManager.ChangePlayerController(newPlayer);
    }

    public void PlayerFell()
    {
        _currentScreen.PlayerFell();
    }

    public void EndLevel()
    {
        player.UnsubscribeEvents();

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private void Update()
    {
        _currentState.UpdateLogic();
        _currentScreen.UpdateLogic();
    }

    private void FixedUpdate()
    {
        _currentState.UpdatePhysics();
        _currentScreen.UpdatePhysics();
    }
}
