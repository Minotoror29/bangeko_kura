using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NemesisPhase
{
    private NemesisPhaseData _data;

    private NemesisController _controller;

    private NemesisState _currentState;

    public NemesisPhaseData Data { get { return _data; } }
    public NemesisController Controller { get { return _controller; } }

    public NemesisPhase(NemesisPhaseData data, NemesisController controller)
    {
        _data = data;
        _controller = controller;
    }

    public void ChangeState(NemesisState nextState)
    {
        _currentState?.Exit();
        _currentState = nextState;
        _currentState.Enter();
    }

    public void Enter()
    {
        ChangeState(new NemesisIdleState(this));
    }

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

        _controller.CheckHealth();
    }
}
