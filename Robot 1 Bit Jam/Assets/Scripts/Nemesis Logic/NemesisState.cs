using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NemesisState : State
{
    private NemesisController _controller;
    private Rigidbody2D _rb;
    private Animator _animator;
    private NewPlayerController _player;

    public NemesisController Controller { get { return _controller; } }
    public Rigidbody2D Rb { get { return _rb; } }
    public Animator Animator { get { return _animator; } }
    public NewPlayerController Player { get { return _player; } }

    public NemesisState(NemesisController controller)
    {
        _controller = controller;
        _rb = controller.Rb;
        _animator = controller.Animator;
        _player = controller.Player;
    }

    public virtual void TakeDamage()
    {

    }
}
