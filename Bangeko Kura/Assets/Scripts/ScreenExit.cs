using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenExit : MonoBehaviour
{
    private GameManager _gameManager;

    [SerializeField] private ScreenManager nextScreen;

    public void Initialize(GameManager gameManager)
    {
        _gameManager = gameManager;
    }

    public void ChangeScreen()
    {
        _gameManager.ChangeScreen(nextScreen);
    }
}
