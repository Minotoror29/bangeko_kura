using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TurretController : Weapon
{
    private List<HealthSystem> _enemiesInRange;

    [SerializeField] private Transform firePoint;
    [SerializeField] private int salvo = 1;
    [SerializeField] private float salvoRate = 1f;
    [SerializeField] private float fireRate = 1.5f;
    [SerializeField] private BulletController bulletPrefab;
    private float _salvoTimer;
    private float _fireTimer;
    private int _bulletsFired;

    //Audio
    private EventInstance _fireSound;

    public override void Initialize(Controller controller, HealthSystem healthSystem)
    {
        base.Initialize(controller, healthSystem);

        _enemiesInRange = new();

        _salvoTimer = 0f;
        _fireTimer = 0f;
        _bulletsFired = 0;

        _fireSound = RuntimeManager.CreateInstance("event:/Weapons/Turret");
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        SortEnemiesInRange();

        if (_salvoTimer < salvoRate)
        {
            _salvoTimer += Time.deltaTime;
        } else
        {
            if (_fireTimer < fireRate)
            {
                _fireTimer += Time.deltaTime;
            } else
            {
                if (_enemiesInRange.Count > 0)
                {
                    Fire();
                    _fireTimer = 0f;

                    if (_bulletsFired == salvo)
                    {
                        _salvoTimer = 0f;
                        _bulletsFired = 0;
                    }
                }
            }
        }
    }

    private void SortEnemiesInRange()
    {
        if (_enemiesInRange.Count == 0) return;

        _enemiesInRange.Sort(CompareDistanceToPlayer);
    }

    private int CompareDistanceToPlayer(HealthSystem a, HealthSystem b)
    {
        float distanceA = (a.transform.position - transform.position).sqrMagnitude;
        float distanceB = (b.transform.position - transform.position).sqrMagnitude;
        return distanceA.CompareTo(distanceB);
    }

    private void Fire()
    {
        BulletController newBullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        newBullet.Initialize(_enemiesInRange[0].transform, Controller.transform);
        _bulletsFired++;

        _fireSound.start();
    }

    private void RemoveEnemyFromTargets(HealthSystem enemy, Transform deathSource)
    {
        _enemiesInRange.Remove(enemy);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out HealthSystem enemy))
        {
            if (!other.gameObject.CompareTag(Controller.gameObject.tag))
            {
                _enemiesInRange.Add(enemy);
                enemy.OnDeath += RemoveEnemyFromTargets;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out HealthSystem enemy) && _enemiesInRange.Contains(enemy))
        {
            RemoveEnemyFromTargets(enemy, Controller.transform);
            enemy.OnDeath -= RemoveEnemyFromTargets;
        }
    }
}
