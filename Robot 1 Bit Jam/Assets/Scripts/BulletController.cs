using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    private Rigidbody _rb;

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

    public void Initialize(Transform target)
    {
        _rb = GetComponent<Rigidbody>();

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

    private void OnCollisionEnter(Collision collision)
    {
        HealthSystem healthSystem = collision.gameObject.GetComponent<HealthSystem>();
        if (healthSystem)
        {
            healthSystem.TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}
