using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private NewPlayerController player;

    [SerializeField] private ScreenManager startScreen;
    [SerializeField] private Transform startSpawnPoint;
    private ScreenManager _currentScreen;

    public event Action OnPlayerFall;
    public event Action OnPlayerDeath;

    private void Start()
    {
        player.Initialize(this);

        foreach (ScreenManager screen in FindObjectsOfType<ScreenManager>(true))
        {
            screen.Initialize(this);
        }

        ChangeScreen(startScreen);

        player.ChangeState(new PlayerSpawnState(player, startSpawnPoint.position));
    }

    public void ChangeScreen(ScreenManager nextScreen)
    {
        _currentScreen?.ExitScreen();
        _currentScreen = nextScreen;
        _currentScreen.EnterScreen();
    }

    public void PlayerFell()
    {
        _currentScreen.PlayerFell();
    }
}
