using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private GameState _currentState;

    [SerializeField] private NewPlayerController player;

    [SerializeField] private bool startOnElevator = false;
    [SerializeField] private Elevator startElevator;
    [SerializeField] private ScreenManager startScreen;
    [SerializeField] private Transform startSpawnPoint;
    private ScreenManager _currentScreen;

    [SerializeField] private CutsceneManager cutsceneManager;
    [SerializeField] private InGameCutsceneManager inGameCutsceneManager;

    [SerializeField] private Canvas gameCanvas;

    public event Action OnInitialize;

    public NewPlayerController Player { get { return player; } }
    public CutsceneManager CutsceneManager { get { return cutsceneManager; } }
    public InGameCutsceneManager InGameCutsceneManager { get { return inGameCutsceneManager; } }
    public Canvas GameCanvas { get { return gameCanvas; } }

    private void Start()
    {
        player.Initialize(this);

        foreach (ScreenManager screen in FindObjectsOfType<ScreenManager>(true))
        {
            screen.Initialize(this);
        }

        if (cutsceneManager != null)
        {
            cutsceneManager.Initialize();
        }
        OnInitialize?.Invoke();

        ChangeScreen(startScreen, null);

        if (!startOnElevator)
        {
            player.ChangeState(new PlayerSpawnState(player, startSpawnPoint.position));
        } else
        {
            player.ChangeState(new PlayerWaitElevatorState(player, startElevator));
            startElevator.ChangeState(ElevatorState.Moving);
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

    public void PlayerFell()
    {
        _currentScreen.PlayerFell();
    }

    public void EndLevel()
    {
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
