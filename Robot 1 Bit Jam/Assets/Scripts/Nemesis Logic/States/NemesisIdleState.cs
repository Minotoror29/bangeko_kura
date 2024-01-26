using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NemesisIdleState : NemesisState
{
    public NemesisIdleState(NemesisPhase phase) : base(phase)
    {
    }

    public override void Enter()
    {
        Animator.CrossFade("Player Idle", 0f);
    }

    public override void Exit()
    {
    }

    public override void OnCollisionEnter(Collision2D collision)
    {
    }

    public override void UpdateLogic()
    {
        if ((Player.transform.position - Controller.transform.position).magnitude <= Controller.SwordDistance && Controller.SwordCooldownTimer <= 0f)
        {
            Phase.ChangeState(new NemesisSwordChargeState(Phase));
        }
        else if ((Player.transform.position - Controller.transform.position).magnitude <= Controller.WalkDistance)
        {
            Phase.ChangeState(new NemesisWalkState(Phase, 3f));
        }
        else if ((Player.transform.position - Controller.transform.position).magnitude <= Controller.ShootDistance)
        {
            Phase.ChangeState(new NemesisShootState(Phase));
        }
        else if ((Player.transform.position - Controller.transform.position).magnitude > Controller.ShootDistance)
        {
            Phase.ChangeState(new NemesisDashState(Phase, 1));
        }
    }

    public override void UpdatePhysics()
    {
        Rb.velocity = Vector2.zero;
    }
}
