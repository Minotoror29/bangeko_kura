using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NemesisIdleState : NemesisState
{
    public NemesisIdleState(NemesisController controller) : base(controller)
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
            Controller.ChangeState(new NemesisSwordChargeState(Controller));
        }
        else if ((Player.transform.position - Controller.transform.position).magnitude <= Controller.WalkDistance)
        {
            Controller.ChangeState(new NemesisWalkState(Controller, 3f));
        }
        else if ((Player.transform.position - Controller.transform.position).magnitude <= Controller.ShootDistance)
        {
            Controller.ChangeState(new NemesisShootState(Controller));
        }
        else if ((Player.transform.position - Controller.transform.position).magnitude > Controller.ShootDistance)
        {
            Controller.ChangeState(new NemesisDashState(Controller));
        }
    }

    public override void UpdatePhysics()
    {
        Rb.velocity = Vector2.zero;
    }
}
