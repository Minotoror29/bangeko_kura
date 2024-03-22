using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Controller : MonoBehaviour
{
    private ScreenManager _screenManager;

    private Rigidbody2D _rb;
    [SerializeField] private Animator generalAnimator;
    [SerializeField] private Animator meshAnimator;
    private HealthSystem _healthSystem;
    [SerializeField] private Transform mesh;
    [SerializeField] private List<SkinnedMeshRenderer> meshRenderers;

    private bool _dashing;

    private List<GameObject> _grounds;

    public ScreenManager ScreenManager { get { return _screenManager; } }
    public Rigidbody2D Rb { get { return _rb; } }
    public Animator GeneralAnimator { get { return generalAnimator; } }
    public Animator MeshAnimator { get { return meshAnimator; } }
    public HealthSystem HealthSystem { get { return _healthSystem; } }
    public Transform Mesh { get { return mesh; } }
    public List<SkinnedMeshRenderer> MeshRenderers { get { return meshRenderers; } }
    public bool Dashing { get { return _dashing; } set { _dashing = value; } }
    public List<GameObject> Grounds { get { return _grounds; } }

    public virtual void Initialize(ScreenManager screenManager)
    {
        _screenManager = screenManager;

        _rb = GetComponent<Rigidbody2D>();
        _healthSystem = GetComponentInChildren<HealthSystem>();
        _healthSystem.Initialize(transform);
        _dashing = false;
        _grounds = new();
    }

    public abstract void UpdateLogic();
    public abstract void UpdatePhysics();

    public virtual bool SwordAttack()
    {
        return false;
    }

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

    public void AddGround(GameObject ground)
    {
        if (ground.CompareTag("Ground") && !_grounds.Contains(ground))
        {
            _grounds.Add(ground);
        }
    }
}
