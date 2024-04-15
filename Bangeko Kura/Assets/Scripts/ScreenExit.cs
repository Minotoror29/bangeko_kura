using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenExit : MonoBehaviour
{
    private GameManager _gameManager;

    [SerializeField] private ScreenManager nextScreen;
    [SerializeField] private Transform relativeSpawnPoint;
    [SerializeField] private GameObject relativeSpawnGround;

    public Transform RelativeSpawnPoint { get { return relativeSpawnPoint; } }
    public GameObject RelativeSpawnGround { get { return relativeSpawnGround; } }

    public void Initialize(GameManager gameManager)
    {
        _gameManager = gameManager;
    }

    public void ChangeScreen()
    {
        _gameManager.ChangeScreen(nextScreen, this);
    }
}
