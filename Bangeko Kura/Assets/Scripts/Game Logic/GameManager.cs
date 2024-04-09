using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private GameState _currentState;

    [SerializeField] private NewPlayerController player;

    [SerializeField] private ScreenManager startScreen;
    [SerializeField] private Transform startSpawnPoint;
    private ScreenManager _currentScreen;

    public NewPlayerController Player { get { return player; } }

    private void Start()
    {
        player.Initialize(this);

        foreach (ScreenManager screen in FindObjectsOfType<ScreenManager>(true))
        {
            screen.Initialize(this);
        }

        ChangeScreen(startScreen);

        player.ChangeState(new PlayerSpawnState(player, startSpawnPoint.position));

        ChangeState(new GamePlayState(this));
    }

    public void ChangeState(GameState nextState)
    {
        _currentState?.Exit();
        _currentState = nextState;
        _currentState.Enter();
    }

    public void ChangeScreen(ScreenManager nextScreen)
    {
        _currentScreen?.ExitScreen();
        _currentScreen = nextScreen;
        _currentScreen.EnterScreen();

        ChangeState(new GameTransitionState(this));
    }

    public void PlayerFell()
    {
        _currentScreen.PlayerFell();
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
