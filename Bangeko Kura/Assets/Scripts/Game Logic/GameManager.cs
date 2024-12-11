using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
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
    [SerializeField] private List<CutsceneManager> cutsceneManagers;
    [SerializeField] private InGameCutsceneManager inGameCutsceneManager;
    [SerializeField] private Transform newPlayerSpawnPoint;

    [SerializeField] private Canvas gameCanvas;
    [SerializeField] private HealthDisplay healthDisplay;

    [SerializeField] private CloudsManager cloudsManager;

    [SerializeField] private CinemachineVirtualCamera deathAnimationCam;
    [SerializeField] private SpriteRenderer blackBackground;
    [SerializeField] private Canvas blackScreen;
    [SerializeField] private Volume negativeVolume;

    private List<BulletController> _bullets;

    public UnityEvent OnStart;
    public event Action OnInitialize;

    public PlayerController Player { get { return player; } }
    public ScreenManager CurrentScreen { get { return _currentScreen; } }
    public List<CutsceneManager> CutsceneManagers { get { return cutsceneManagers; } }
    public InGameCutsceneManager InGameCutsceneManager { get { return inGameCutsceneManager; } }
    public Canvas GameCanvas { get { return gameCanvas; } }
    public HealthDisplay HealthDisplay { get {  return healthDisplay; } }
    public CloudsManager CloudsManager { get { return cloudsManager; } }
    public CinemachineVirtualCamera DeathAnimationCam {  get { return deathAnimationCam; } }
    public SpriteRenderer BlackBackground { get { return blackBackground; } }
    public Canvas BlackScreen { get { return blackScreen; } }
    public Volume NegativeVolume {  get { return negativeVolume; } }
    public List<BulletController> Bullets { get { return _bullets; } }

    private void Start()
    {
        player.Initialize(this);

        foreach (ScreenManager screen in FindObjectsOfType<ScreenManager>(true))
        {
            screen.Initialize(this, player);
        }

        foreach (CutsceneManager cutsceneManager in cutsceneManagers)
        {
            cutsceneManager.Initialize();
        }
        OnInitialize?.Invoke();

        _currentScreen = startScreen;
        _currentScreen.EnterScreen();

        switch (startState)
        {
            case StartingPlayerState.Idle:
                player.ChangeState(new PlayerSpawnState(player, startSpawnPoint.position));
                break;
            case StartingPlayerState.Elevator:
                player.ChangeState(new PlayerWaitElevatorState(player, startElevator));
                startElevator.ChangeState(ElevatorState.Moving);
                break;
            case StartingPlayerState.Falling:
                player.ChangeState(new PlayerLandState(player, startSpawnPoint.position, startGround));
                break;
        }

        _bullets = new();

        OnStart?.Invoke();

        ChangeState(new GamePlayState(this));
    }

    public void ChangeState(GameState nextState)
    {
        _currentState?.Exit();
        _currentState = nextState;
        _currentState.Enter();
    }

    public void ChangeToCutsceneState(CutsceneManager cutsceneManager)
    {
        _currentState?.Exit();
        _currentState = new GameCutsceneState(this, cutsceneManager);
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

    public void OnPlayerDeath(bool fromFall)
    {
        ChangeState(new GameDeathState(this, fromFall));
    }

    public void EndLevel()
    {
        player.UnsubscribeEvents();

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public ParticleSystem[] GetAllParticles()
    {
        return FindObjectsOfType<ParticleSystem>();
    }

    private void Update()
    {
        _currentState.UpdateLogic();
    }

    private void FixedUpdate()
    {
        _currentState.UpdatePhysics();
    }
}
