using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesManager : MonoBehaviour
{
    private List<EnemyController> _enemies;

    [SerializeField] private PlayerController player;

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
        foreach (EnemyController enemy in FindObjectsOfType<EnemyController>())
        {
            _enemies.Add(enemy);
        }

        foreach (EnemyController enemy in _enemies)
        {
            enemy.Initialize(this, player);
        }
    }

    public void UpdateLogic()
    {
        foreach (EnemyController enemy in _enemies)
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
