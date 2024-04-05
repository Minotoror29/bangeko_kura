using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerState : State
{
    private NewPlayerController _controller;
    private Rigidbody2D _rb;
    private PlayerControls _controls;
    private Animator _animator;

    public NewPlayerController Controller { get { return _controller; } }
    public Rigidbody2D Rb { get { return _rb; } }
    public PlayerControls Controls { get { return _controls; } }
    public Animator Animator { get { return _animator; } }

    public PlayerState(NewPlayerController controller)
    {
        _controller = controller;
        _rb = controller.Rb;
        _controls = controller.Controls;
        _animator = controller.MeshAnimator;
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
