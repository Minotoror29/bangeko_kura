using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemiesManager : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private ScreenManager screenManager;

    private List<EnemyController> _enemies;

    private PlayerController _player;

    public event Action OnAllEnemiesDead;

    private void OnEnable()
    {
        screenManager.OnInitialize += Initialize;
        screenManager.OnUpdate += UpdateLogic;
        screenManager.OnFixedUpdate += UpdatePhysics;
        screenManager.OnEnter += SetActiveEnemies;
        screenManager.OnExit += SetActiveEnemies;
        screenManager.OnChangePlayerController += ChangePlayerController;
        screenManager.OnPauseScreen += PauseEnemies;
    }

    private void OnDisable()
    {
        screenManager.OnInitialize -= Initialize;
        screenManager.OnUpdate -= UpdateLogic;
        screenManager.OnFixedUpdate -= UpdatePhysics;
        screenManager.OnEnter -= SetActiveEnemies;
        screenManager.OnExit -= SetActiveEnemies;
        screenManager.OnChangePlayerController -= ChangePlayerController;
        screenManager.OnPauseScreen -= PauseEnemies;
    }

    public void Initialize(bool isArena)
    {
        _enemies = new();
        _player = FindObjectOfType<PlayerController>();
        foreach (EnemyController enemy in GetComponentsInChildren<EnemyController>(true))
        {
            _enemies.Add(enemy);
            enemy.Initialize(this, _player, gameManager);
            enemy.gameObject.SetActive(isArena);
        }

        screenManager.OnPlayerDeath += PlayerDied;
    }

    public void ChangePlayerController(PlayerController newPlayer)
    {
        _player = newPlayer;

        foreach (EnemyController enemy in _enemies)
        {
            enemy.ChangePlayerController(newPlayer);
        }
    }

    private void PlayerDied()
    {
        foreach (EnemyController enemy in _enemies)
        {
            enemy.PlayerDied();
        }
    }

    public void UpdateLogic()
    {
        List<EnemyController> enemies = new();
        foreach (EnemyController enemy in _enemies)
        {
            enemies.Add(enemy);
        }

        foreach (EnemyController enemy in enemies)
        {
            enemy.UpdateLogic();
        }
    }

    public void UpdatePhysics()
    {
        foreach (EnemyController enemy in _enemies)
        {
            enemy.UpdatePhysics();
        }
    }

    public void RemoveEnemy(EnemyController enemy)
    {
        if (_enemies.Contains(enemy))
        {
            _enemies.Remove(enemy);
        }

        if (_enemies.Count == 0)
        {
            OnAllEnemiesDead?.Invoke();
        }
    }

    public void SetActiveEnemies(bool active)
    {
        foreach (EnemyController enemy in _enemies)
        {
            enemy.Activate(active);
        }
    }

    public void PauseEnemies()
    {
        foreach (EnemyController enemy in _enemies)
        {
            enemy.Rb.velocity = Vector2.zero;
        }
    }

    public List<EnemyController> EnemiesCloseTo(EnemyController source, float distance)
    {
        List<EnemyController> closeEnemies = new();

        foreach (EnemyController enemy in _enemies)
        {
            if (enemy != source)
            {
                if ((enemy.transform.position - source.transform.position).magnitude <= distance)
                {
                    closeEnemies.Add(enemy);
                }
            }
        }

        return closeEnemies;
    }
}
