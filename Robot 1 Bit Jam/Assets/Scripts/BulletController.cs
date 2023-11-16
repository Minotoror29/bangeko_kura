using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    private Rigidbody _rb;
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
        _rb = GetComponent<Rigidbody>();
        _source = source;

        _lifeTimer = 0f;

        transform.LookAt(target);
        transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);
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
        _rb.velocity = speed * Time.fixedDeltaTime * transform.forward;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out HealthSystem healthSystem))
        {
            if (_source != other.transform)
            {
                healthSystem.TakeDamage(damage, _source);
                Destroy(gameObject);
            }
        } else if (other.TryGetComponent(out ShieldController shield))
        {
            if (_source != shield.Controller.transform)
            {
                shield.TakeDamage(_source);
                Destroy(gameObject);
            }
        } else if (other.gameObject.CompareTag("Obstacle"))
        {
            Destroy(gameObject);
        }
    }
}
