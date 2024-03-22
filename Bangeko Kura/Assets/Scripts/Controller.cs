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

    [SerializeField] private float changeColorTime;
    [SerializeField] private Color damageColor;
    private float _changeColorTimer = 0f;

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
        HealthSystem.OnDamage += TakeDamage;

        _dashing = false;
        _grounds = new();
    }

    public virtual void UpdateLogic()
    {
        if (_changeColorTimer > 0f)
        {
            _changeColorTimer -= Time.deltaTime;
        }
        else
        {
            foreach (SkinnedMeshRenderer renderer in MeshRenderers)
            {
                renderer.material.SetColor("_Dark_Color", Color.black);
            }
        }
    }

    public abstract void UpdatePhysics();

    public virtual bool SwordAttack()
    {
        return false;
    }

    private void TakeDamage(int amount)
    {
        GeneralAnimator.SetTrigger("Squish");
        ChangeColor();
    }

    private void ChangeColor()
    {
        foreach (SkinnedMeshRenderer renderer in MeshRenderers)
        {
            renderer.material.SetColor("_Dark_Color", damageColor);
        }

        _changeColorTimer = changeColorTime;
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
