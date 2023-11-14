using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordController : Weapon
{
    [Tooltip("Put -1 for instant kill"), SerializeField] private int damage;
    [SerializeField] private float cooldown = 1f;
    private float _cooldownTimer;

    private List<HealthSystem> _enemiesInRange;

    public override void Initialize(Transform controller)
    {
        base.Initialize(controller);

        _enemiesInRange = new();
        _cooldownTimer = cooldown;
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
                foreach (HealthSystem enemy in _enemiesInRange)
                {
                    if (damage > 0)
                    {
                        enemy.TakeDamage(damage, Controller);
                    }
                    else if (damage == -1)
                    {
                        enemy.Die(Controller);
                    }
                }

                _cooldownTimer = 0f;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out HealthSystem healthSystem))
        {
            _enemiesInRange.Add(healthSystem);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out HealthSystem healthSystem))
        {
            _enemiesInRange.Remove(healthSystem);
        }
    }
}
