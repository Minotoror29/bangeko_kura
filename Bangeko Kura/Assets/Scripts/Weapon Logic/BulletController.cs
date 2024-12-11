using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Knockback
{
    public float knockbackDistance;
    public float knockbackSpeed;
}

public class BulletController : MonoBehaviour
{
    private Rigidbody2D _rb;
    private Transform _source;

    [SerializeField] private float speed = 500f;
    [SerializeField] private float lifetime = 5f;
    private float _lifeTimer;

    [SerializeField] private int damage = 1;
    [SerializeField] private float knockbackDistance = 1f;
    [SerializeField] private float knockbackSpeed = 100f;
    private Knockback _knockback;

    [Header("Effects")]
    [SerializeField] private List<Effect> fireEffects;

    [Space]
    [SerializeField] private List<Effect> impactEffects;

    [Space]
    [SerializeField] private Effect smokeEffect;

    [Space]
    [SerializeField] private Effect fireParticles;
    [SerializeField] private Effect impactParticles;

    [Header("Audio")]
    [SerializeField] private string bulletSoundPath;
    private EventInstance _bulletSound;

    public Rigidbody2D Rb { get { return _rb; } }

    public void Initialize(Vector2 direction, Transform source)
    {
        _rb = GetComponent<Rigidbody2D>();
        _source = source;

        _lifeTimer = 0f;

        transform.SetPositionAndRotation((Vector2)transform.position + direction * 0.015f, Quaternion.LookRotation(transform.forward, direction.normalized));
        int randomFireEffect = Random.Range(0, fireEffects.Count);
        Effect newFireEffect = Instantiate(fireEffects[randomFireEffect], transform.position, transform.rotation);
        newFireEffect.Initialize();

        Effect newFireParticles = Instantiate(fireParticles, transform.position, transform.rotation);
        newFireParticles.Initialize();

        Effect newSmokeEffect = Instantiate(smokeEffect, transform.position, transform.rotation);
        newSmokeEffect.Initialize();

        _knockback = new Knockback
        {
            knockbackDistance = knockbackDistance,
            knockbackSpeed = knockbackSpeed
        };

        _bulletSound = RuntimeManager.CreateInstance(bulletSoundPath);
        _bulletSound.start();
    }

    public void DestroyBullet()
    {
        Destroy(gameObject);
    }

    public void UpdateLogic()
    {
        if (_lifeTimer < lifetime)
        {
            _lifeTimer += Time.deltaTime;
        } else
        {
            gameObject.SetActive(false);
        }
    }

    public void UpdatePhysics()
    {
        _rb.velocity = speed * Time.fixedDeltaTime * transform.up;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out HealthSystem healthSystem))
        {
            if (_source != healthSystem.Source)
            {
                int randomImpactEffect = Random.Range(0, impactEffects.Count);
                Effect newImpactEffect = Instantiate(impactEffects[randomImpactEffect], transform.position, transform.rotation);
                newImpactEffect.Initialize();

                Effect newImpactParticles = Instantiate(impactParticles, transform.position, transform.rotation);
                newImpactParticles.Initialize();

                healthSystem.TakeDamage(damage, _source, _knockback, DamageCause.Turret);
                gameObject.SetActive(false);
            }
        }
        else if (collision.TryGetComponent(out ShieldController shield))
        {
            if (_source != shield.Controller.transform)
            {
                shield.TakeDamage(_source, _knockback);
                gameObject.SetActive(false);
            }
        }
        else if (collision.gameObject.CompareTag("Obstacle"))
        {
            gameObject.SetActive(false);
        }
    }
}
