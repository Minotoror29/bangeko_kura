using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Controller : MonoBehaviour
{
    private Rigidbody2D _rb;
    [SerializeField] private Animator animator;
    private HealthSystem _healthSystem;

    private bool _dashing;

    private List<GameObject> _grounds;

    public Rigidbody2D Rb { get { return _rb; } }
    public Animator Animator { get { return animator; } }
    public HealthSystem HealthSystem { get { return _healthSystem; } }
    public bool Dashing { get { return _dashing; } set { _dashing = value; } }
    public List<GameObject> Grounds { get { return _grounds; } }

    public virtual void Initialize()
    {
        _rb = GetComponent<Rigidbody2D>();
        _healthSystem = GetComponentInChildren<HealthSystem>();
        _healthSystem.Initialize(transform);
        _dashing = false;
        _grounds = new();
    }

    public abstract void UpdateLogic();
    public abstract void UpdatePhysics();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") && !_grounds.Contains(collision.gameObject))
        {
            _grounds.Add(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (_grounds.Contains(collision.gameObject))
        {
            _grounds.Remove(collision.gameObject);
        }
    }
}
