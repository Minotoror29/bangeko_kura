using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NemesisPhase
{
    private NemesisPhaseData _data;

    private NemesisController _controller;
    private NewPlayerController _player;

    private NemesisState _currentState;

    public NemesisPhaseData Data { get { return _data; } }
    public NemesisController Controller { get { return _controller; } }
    public NewPlayerController Player { get { return _player; } }

    public NemesisPhase(NemesisPhaseData data, NemesisController controller)
    {
        _data = data;
        _controller = controller;
        _player = controller.Player;
    }

    public void ChangeState(NemesisState nextState)
    {
        _currentState?.Exit();
        _currentState = nextState;
        _currentState.Enter();
    }

    public abstract void Enter();

    public void Exit()
    {
        _currentState?.Exit();
    }

    public void OnCollisionEnter(Collision2D collision)
    {
        _currentState.OnCollisionEnter(collision);
    }

    public void UpdateLogic()
    {
        _currentState.UpdateLogic();
    }

    public void UpdatePhysics()
    {
        _currentState.UpdatePhysics();
    }

    public void TakeDamage()
    {
        _currentState.TakeDamage();

        //controller.CheckHealth();
    }
}
