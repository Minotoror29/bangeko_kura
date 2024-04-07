using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemiesManager : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private ScreenManager screenManager;

    private List<EnemyController> _enemies;

    private NewPlayerController _player;

    private void Start()
    {
        Initialize();
    }

    private void Update()
    {
        UpdateLogic();
    }

    private void FixedUpdate()
    {
        UpdatePhysics();
    }

    public void Initialize()
    {
        _enemies = new();
        _player = FindObjectOfType<NewPlayerController>();
        foreach (EnemyController enemy in FindObjectsOfType<EnemyController>())
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
            //EndLevel();
        }
    }

    private void EndLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
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
