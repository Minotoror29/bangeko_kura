using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerState : State
{
    private PlayerController _controller;
    private Rigidbody2D _rb;
    private Animator _animator;

    public PlayerController Controller { get { return _controller; } }
    public Rigidbody2D Rb { get { return _rb; } }
    public Animator Animator { get { return _animator; } }

    public PlayerState(PlayerController controller)
    {
        _controller = controller;
        _rb = controller.Rb;
        _animator = controller.MeshAnimator;
    }

    public abstract void OnTriggerEnter(Collider2D collision);

    public virtual void OnTriggerStay(Collider2D collision)
    {

    }

    public virtual bool CanDash()
    {
        return false;
    }

    public virtual bool CanAttackSword()
    {
        return false;
    }

    public virtual bool CanBeKnockbacked()
    {
        return true;
    }
}
