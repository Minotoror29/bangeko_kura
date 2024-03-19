using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    private Rigidbody2D _rb;
    private Transform _source;

    [SerializeField] private float speed = 500f;
    [SerializeField] private float lifetime = 5f;
    private float _lifeTimer;

    [SerializeField] private int damage = 1;

    [Header("Effects")]
    [SerializeField] private List<GameObject> fireEffects;
    [SerializeField] private float fireEffectLifetime = 0.1f;

    [Space]
    [SerializeField] private List<GameObject> impactEffects;
    [SerializeField] private float impactEffectLifetime = 0.2f;

    private void Update()
    {
        UpdateLogic();
    }

    private void FixedUpdate()
    {
        UpdatePhysics();
    }

    public void Initialize(Vector2 direction, Transform source)
    {
        _rb = GetComponent<Rigidbody2D>();
        _source = source;

        _lifeTimer = 0f;

        transform.position = (Vector2)transform.position + direction * 0.015f;
        transform.rotation = Quaternion.LookRotation(transform.forward, direction.normalized);

        int randomFireEffect = Random.Range(0, fireEffects.Count);
        GameObject newFireEffect = Instantiate(fireEffects[randomFireEffect], transform.position, transform.rotation);
        Destroy(newFireEffect, fireEffectLifetime);
    }

    public void UpdateLogic()
    {
        if (_lifeTimer < lifetime)
        {
            _lifeTimer += Time.deltaTime;
        } else
        {
            Destroy(gameObject);
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
                GameObject newImpactEffect = Instantiate(impactEffects[randomImpactEffect], transform.position, Quaternion.identity);
                Destroy(newImpactEffect, impactEffectLifetime);
                healthSystem.TakeDamage(damage, _source);
                Destroy(gameObject);
            }
        }
        else if (collision.TryGetComponent(out ShieldController shield))
        {
            if (_source != shield.Controller.transform)
            {
                shield.TakeDamage(_source);
                Destroy(gameObject);
            }
        }
        else if (collision.gameObject.CompareTag("Obstacle"))
        {
            Destroy(gameObject);
        }
    }
}
