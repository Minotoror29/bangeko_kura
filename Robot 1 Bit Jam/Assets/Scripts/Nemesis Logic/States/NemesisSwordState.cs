using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NemesisSwordState : NemesisState
{
    private NemesisSwordSubstate _currentSubstate;

    public NemesisSwordState(NemesisController controller) : base(controller)
    {
    }

    public override void Enter()
    {
        ChangeSubstate(new NemesisSwordChargeState(Controller, this));
    }

    public override void Exit()
    {
    }

    public override void OnCollisionEnter(Collision2D collision)
    {
        _currentSubstate.OnCollisionEnter(collision);
    }

    public override void UpdateLogic()
    {
        _currentSubstate.UpdateLogic();
    }

    public override void UpdatePhysics()
    {
        _currentSubstate.UpdatePhysics();
    }

    public void ChangeSubstate(NemesisSwordSubstate nextSubstate)
    {
        _currentSubstate?.Exit();
        _currentSubstate = nextSubstate;
        _currentSubstate?.Enter();
    }

    public void CheckNextState()
    {
        Controller.ChangeState(new NemesisIdleState(Controller));
    }
}
