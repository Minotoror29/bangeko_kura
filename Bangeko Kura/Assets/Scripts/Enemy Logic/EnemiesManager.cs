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
    }

    private void OnDisable()
    {
        screenManager.OnInitialize -= Initialize;
        screenManager.OnUpdate -= UpdateLogic;
        screenManager.OnFixedUpdate -= UpdatePhysics;
        screenManager.OnEnter -= SetActiveEnemies;
        screenManager.OnExit -= SetActiveEnemies;
    }

    public void Initialize()
    {
        _enemies = new();
        _player = FindObjectOfType<PlayerController>();
        foreach (EnemyController enemy in GetComponentsInChildren<EnemyController>(true))
        {
            _enemies.Add(enemy);
            enemy.Initialize(this, _player, gameManager);
        }

        screenManager.OnPlayerDeath += PlayerDied;
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

    private void SetActiveEnemies(bool active)
    {
        foreach (EnemyController enemy in _enemies)
        {
            enemy.gameObject.SetActive(active);
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
