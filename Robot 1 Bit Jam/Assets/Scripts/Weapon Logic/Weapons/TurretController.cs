using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TurretController : Weapon
{
    private List<EnemyController> _enemiesInRange;

    [SerializeField] private Transform firePoint;
    [Tooltip("Projectiles per second"), SerializeField] private float fireRate = 1.5f;
    [SerializeField] private BulletController bulletPrefab;
    private float _fireTimer;

    public override void Initialize(Transform controller)
    {
        base.Initialize(controller);

        _enemiesInRange = new();

        _fireTimer = 0f;
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

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
        newBullet.Initialize(_enemiesInRange[0].transform, Controller);
    }

    private void RemoveEnemyFromTargets(EnemyController enemy, Transform deathSource)
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
            RemoveEnemyFromTargets(enemy, Controller);
            enemy.OnDeath -= RemoveEnemyFromTargets;
        }
    }
}
