using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NemesisState : State
{
    private NemesisPhase _phase;
    private NemesisController _controller;
    private Rigidbody2D _rb;
    private Animator _animator;
    private PlayerController _player;

    public NemesisPhase Phase { get { return _phase; } }
    public NemesisController Controller { get { return _controller; } }
    public Rigidbody2D Rb { get { return _rb; } }
    public Animator Animator { get { return _animator; } }
    public PlayerController Player { get { return _player; } }

    public NemesisState(NemesisPhase phase)
    {
        _phase = phase;
        _controller = phase.Controller;
        _rb = phase.Controller.Rb;
        _animator = phase.Controller.MeshAnimator;
        _player = phase.Controller.Player;
    }

    public virtual void TakeDamage()
    {

    }
}
