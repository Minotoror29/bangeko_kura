using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NemesisDashState : NemesisState
{
    private int _dashNumber;

    private Vector2 _dashDirection;
    private Vector2 _dashOrigin;

    public NemesisDashState(NemesisPhase phase, int dashNumber) : base(phase)
    {
        _dashNumber = dashNumber;
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
            if ((Player.transform.position - Controller.transform.position).magnitude <= Controller.WalkDistance)
            {
                Phase.ChangeState(new NemesisSwordAttackState(Phase));
            } else if ((Player.transform.position - Controller.transform.position).magnitude <= Controller.ShootDistance)
            {
                Phase.ChangeState(new NemesisShootState(Phase));
            } else
            {
                if (_dashNumber == 1)
                {
                    Phase.ChangeState(new NemesisDashState(Phase, 2));
                } else if (_dashNumber == 2)
                {
                    Phase.ChangeState(new NemesisShootState(Phase));
                }
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
