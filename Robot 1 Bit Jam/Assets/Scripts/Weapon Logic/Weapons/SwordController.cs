using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordController : Weapon
{
    [Tooltip("Put -1 for instant kill"), SerializeField] private int damage;
    [SerializeField] private float cooldown = 1f;
    private float _cooldownTimer;

    private List<HealthSystem> _enemiesInRange;
    private List<HealthSystem> _alliesInRange;

    private EventInstance _swordSound;

    public override void Initialize(Controller controller, HealthSystem healthSystem)
    {
        base.Initialize(controller, healthSystem);

        _cooldownTimer = cooldown;
        _enemiesInRange = new();
        _alliesInRange = new();

        HealthSystem.OnDeath += RemoveFromOthersTargets;

        _swordSound = RuntimeManager.CreateInstance("event:/Weapons/Sword");
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        if (_cooldownTimer < cooldown)
        {
            _cooldownTimer += Time.deltaTime;
        } else
        {
            if (_enemiesInRange.Count > 0)
            {
                if (Controller.Animator != null)
                {
                    Controller.Animator.SetTrigger("Sword");
                }

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
                    if (damage > 0)
                    {
                        target.TakeDamage(damage, Controller.transform);
                    }
                    else if (damage == -1)
                    {
                        target.Die(Controller.transform);
                    }
                }

                _cooldownTimer = 0f;

                _swordSound.start();
            }
        }
    }

    private void RemoveTarget(HealthSystem target, Transform deathSource)
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

    private void RemoveFromOthersTargets(HealthSystem target, Transform deathSource)
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
            RemoveTarget(healthSystem, Controller.transform);
            healthSystem.OnDeath -= RemoveTarget;
        }
    }
}
