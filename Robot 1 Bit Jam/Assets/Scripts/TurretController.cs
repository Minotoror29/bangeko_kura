using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TurretController : MonoBehaviour
{
    private List<EnemyController> _enemiesInRange;

    [SerializeField] private Transform firePoint;
    [Tooltip("Projectiles per second"), SerializeField] private float fireRate = 1f;
    [SerializeField] private BulletController bulletPrefab;
    private float _fireTimer;

    private void Start()
    {
        Initialize();
    }

    private void Update()
    {
        UpdateLogic();
    }

    public void Initialize()
    {
        _enemiesInRange = new();

        _fireTimer = 0f;
    }

    public void UpdateLogic()
    {
        SortEnemiesInRange();

        if (_fireTimer < 1f / fireRate)
        {
            _fireTimer += Time.deltaTime;
        } else
        {
            if (_enemiesInRange.Count > 0)
            {
                Fire();
                _fireTimer = 0f;
            }
        }
    }

    private void SortEnemiesInRange()
    {
        if (_enemiesInRange.Count == 0) return;

        _enemiesInRange.Sort(CompareDistanceToPlayer);
    }

    private int CompareDistanceToPlayer(EnemyController a, EnemyController b)
    {
        float distanceA = (a.transform.position - transform.position).sqrMagnitude;
        float distanceB = (b.transform.position - transform.position).sqrMagnitude;
        return distanceA.CompareTo(distanceB);
    }

    private void Fire()
    {
        BulletController newBullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        newBullet.Initialize(_enemiesInRange[0].transform);
    }

    private void RemoveEnemyFromTargets(EnemyController enemy)
    {
        _enemiesInRange.Remove(enemy);
    }

    private void OnTriggerEnter(Collider other)
    {
        EnemyController enemy = other.GetComponent<EnemyController>();
        if (enemy)
        {
            _enemiesInRange.Add(enemy);
            enemy.OnDeath += RemoveEnemyFromTargets;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        EnemyController enemy = other.GetComponent<EnemyController>();
        if (enemy && _enemiesInRange.Contains(enemy))
        {
            RemoveEnemyFromTargets(enemy);
            enemy.OnDeath -= RemoveEnemyFromTargets;
        }
    }
}
