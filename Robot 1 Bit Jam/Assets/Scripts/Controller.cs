using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Controller : MonoBehaviour
{
    private Rigidbody _rb;
    [SerializeField] private Animator animator;

    private bool _dashing;

    public Rigidbody Rb { get { return _rb; } }
    public Animator Animator { get { return animator; } }
    public bool Dashing { get { return _dashing; } set { _dashing = value; } }

    public virtual void Initialize()
    {
        _rb = GetComponent<Rigidbody>();
        _dashing = false;
    }

    public abstract void UpdateLogic();
    public abstract void UpdatePhysics();
}
