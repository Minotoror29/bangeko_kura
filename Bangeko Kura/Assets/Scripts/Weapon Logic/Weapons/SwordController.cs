using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SwordController : Weapon
{
    [SerializeField] private bool automatic = true;

    [Space]
    [Tooltip("Put -1 for instant kill"), SerializeField] private int damage;
    [SerializeField] private float cooldown = 1f;
    [SerializeField] private float buildupTime;
    [SerializeField] private float swordKnockbackDistance;
    [SerializeField] private float swordKnockbackSpeed;
    private float _cooldownTimer = 0f;
    private Knockback _swordKnockback;

    private List<HealthSystem> _enemiesInRange;
    private List<HealthSystem> _alliesInRange;

    [SerializeField] private GameObject swordEffectPrefab;
    [SerializeField] private float swordEffectTime = 0.2f;
    [SerializeField] private GameObject swordSmokePrefab;
    [SerializeField] private GameObject swordImpactParticles;

    private EventInstance _swordSound;

    public override void Initialize(Controller controller, HealthSystem healthSystem)
    {
        base.Initialize(controller, healthSystem);

        _enemiesInRange = new();
        _alliesInRange = new();

        HealthSystem.OnDeath += RemoveFromOthersTargets;

        _swordKnockback = new Knockback { knockbackDistance = swordKnockbackDistance, knockbackSpeed = swordKnockbackSpeed };

        _swordSound = RuntimeManager.CreateInstance("event:/Weapons/Sword");
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        if (_cooldownTimer > 0f)
        {
            _cooldownTimer -= Time.deltaTime;
        } else if (automatic)
        {
            if (_enemiesInRange.Count > 0)
            {
                if (!Controller.SwordAttack(buildupTime))
                {
                    return;
                }

                StartCoroutine(Buildup());
            }
        }
    }

    public void SwordStrike()
    {
        if (_cooldownTimer > 0f || !gameObject.activeSelf) return;

        if (!Controller.SwordAttack(buildupTime)) return;

        StartCoroutine(Buildup());
    }

    private IEnumerator Buildup()
    {
        yield return new WaitForSeconds(buildupTime);

        List<HealthSystem> targets = new();
        foreach (HealthSystem enemy in _enemiesInRange)
        {
            targets.Add(enemy);
        }

        foreach (HealthSystem ally in _alliesInRange)
        {
            targets.Add(ally);
        }

        foreach (HealthSystem target in targets)
        {
            target.TakeDamage(damage, Controller.transform, _swordKnockback, DamageCause.Sword);
            GameObject newParticles = Instantiate(swordImpactParticles, target.transform.position, Quaternion.identity);
            Destroy(newParticles, 5f);
        }

        _cooldownTimer = cooldown;

        _swordSound.start();

        GameObject newSwordEffect = Instantiate(swordEffectPrefab, Controller.transform.position, Quaternion.Euler(new Vector3(0f, 0f, -Controller.Mesh.transform.rotation.eulerAngles.y)));
        Destroy(newSwordEffect, swordEffectTime);

        GameObject newSwordSmoke = Instantiate(swordSmokePrefab, Controller.transform.position, Quaternion.Euler(new Vector3(0f, 0f, -Controller.Mesh.transform.rotation.eulerAngles.y)));
        Destroy(newSwordSmoke, 0.25f);
    }

    private void RemoveTarget(HealthSystem target, Transform deathSource, DamageCause damageCause)
    {
        if (target.gameObject.CompareTag(Controller.gameObject.tag))
        {
            _alliesInRange.Remove(target);
        }
        else
        {
            _enemiesInRange.Remove(target);
        }
    }

    private void RemoveFromOthersTargets(HealthSystem target, Transform deathSource, DamageCause damageCause)
    {
        foreach (HealthSystem enemy in _enemiesInRange)
        {
            enemy.OnDeath -= RemoveTarget;
        }

        foreach (HealthSystem ally in _alliesInRange)
        {
            ally.OnDeath -= RemoveTarget;
        }
    }

    public void SetActiveColliders(bool active)
    {
        GetComponent<CircleCollider2D>().enabled = active;

        if (!active)
        {
            _enemiesInRange.Clear();
            _alliesInRange.Clear();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out HealthSystem healthSystem))
        {
            if (healthSystem.gameObject.CompareTag(Controller.gameObject.tag) && healthSystem != HealthSystem)
            {
                _alliesInRange.Add(healthSystem);
            }
            else
            {
                _enemiesInRange.Add(healthSystem);
            }
            healthSystem.OnDeath += RemoveTarget;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out HealthSystem healthSystem))
        {
            RemoveTarget(healthSystem, Controller.transform, DamageCause.Other);
            healthSystem.OnDeath -= RemoveTarget;
        }
    }
}
