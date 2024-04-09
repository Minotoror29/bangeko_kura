using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameState : State
{
    private GameManager _gameManager;

    public GameManager GameManager { get { return _gameManager; } }

    public GameState(GameManager gameManager)
    {
        _gameManager = gameManager;
    }
}
