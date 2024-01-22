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
        Rb.velocity = Vector2.zero;
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
        if ((Player.transform.position - Controller.transform.position).magnitude < Controller.SwordDistance && Controller.SwordCooldownTimer <= 0f)
        {
            Controller.ChangeState(new NemesisSwordState(Controller));
        }
    }

    public override void UpdatePhysics()
    {
    }
}
