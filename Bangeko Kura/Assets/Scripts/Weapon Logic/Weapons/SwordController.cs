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

    [SerializeField] private GameObject swordEffectPrefab;
    [SerializeField] private float swordEffectTime = 0.2f;
    private GameObject _swordEffect;
    private float _swordEffectTimer;

    private EventInstance _swordSound;

    public override void Initialize(Controller controller, HealthSystem healthSystem)
    {
        base.Initialize(controller, healthSystem);

        _cooldownTimer = cooldown;
        _enemiesInRange = new();
        _alliesInRange = new();

        HealthSystem.OnDeath += RemoveFromOthersTargets;

        _swordEffect = Instantiate(swordEffectPrefab);
        _swordEffect.SetActive(false);
        _swordEffectTimer = 0f;

        _swordSound = RuntimeManager.CreateInstance("event:/Weapons/Sword");
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        _swordEffect.transform.position = Controller.transform.position;
        _swordEffect.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, Controller.Mesh.transform.rotation.eulerAngles.y));

        if (_swordEffect.activeSelf)
        {
            if (_swordEffectTimer > 0f)
            {
                _swordEffectTimer -= Time.deltaTime;
            } else
            {
                _swordEffect.SetActive(false);
            }
        }

        if (_cooldownTimer < cooldown)
        {
            _cooldownTimer += Time.deltaTime;
        } else
        {
            if (_enemiesInRange.Count > 0)
            {
                if (!Controller.SwordAttack())
                {
                    return;
                }

                if (Controller.MeshAnimator != null)
                {
                    //Controller.Animator.SetTrigger("Sword");
                    //Controller.Animator.CrossFade("Player Sword", 0f);
                    //Controller.SwordAttack();
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
                    target.TakeDamage(damage, Controller.transform);
                }

                _cooldownTimer = 0f;

                _swordSound.start();

                _swordEffect.SetActive(true);
                _swordEffectTimer = swordEffectTime;
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
