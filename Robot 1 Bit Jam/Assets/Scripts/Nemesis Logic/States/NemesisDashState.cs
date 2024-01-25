using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NemesisDashState : NemesisState
{
    private Vector2 _dashDirection;
    private Vector2 _dashOrigin;

    public NemesisDashState(NemesisPhase phase) : base(phase)
    {
    }

    public override void Enter()
    {
        _dashDirection = Player.transform.position - Controller.transform.position;
        _dashOrigin = Controller.transform.position;

        Animator.CrossFade("Player Dash", 0f);
    }

    public override void Exit()
    {
    }

    public override void OnCollisionEnter(Collision2D collision)
    {
    }

    public override void UpdateLogic()
    {
        if (((Vector2)Controller.transform.position - _dashOrigin).magnitude >= Controller.DashDistance)
        {
            if ((Player.transform.position - Controller.transform.position).magnitude <= Controller.SwordDistance)
            {
                Phase.ChangeState(new NemesisSwordAttackState(Phase));
            } else
            {
                Phase.ChangeState(new NemesisWalkState(Phase, Phase.Data.pauseAfterDash));
            }
        }
    }

    public override void UpdatePhysics()
    {
        Controller.MoveTowards(_dashDirection, Controller.DashSpeed);
    }

    public override void TakeDamage()
    {
        base.TakeDamage();

        Phase.ChangeState(new NemesisStunState(Phase));
    }
}
