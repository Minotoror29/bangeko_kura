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

    private void Update()
    {
        UpdateLogic();
    }

    private void FixedUpdate()
    {
        UpdatePhysics();
    }

    public void Initialize(Transform target, Transform source)
    {
        _rb = GetComponent<Rigidbody2D>();
        _source = source;

        _lifeTimer = 0f;

        transform.rotation = Quaternion.LookRotation(transform.forward, target.position - transform.position);
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
