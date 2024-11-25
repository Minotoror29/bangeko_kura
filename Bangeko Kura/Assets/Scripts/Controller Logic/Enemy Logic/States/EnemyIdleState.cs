using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : EnemyState
{
    public EnemyIdleState(EnemyController controller) : base(controller)
    {
        Id = EnemyStateId.Idle;
    }

    public override void Enter()
    {
        base.Enter();

        Controller.MeshAnimator.CrossFade("Enemy Idle", 0f);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void OnCollisionEnter(Collision2D collision)
    {
    }

    public override void OnCollisionStay(Collision2D collision)
    {
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        if (Controller.Grounds.Count == 0)
        {
            Controller.ChangeState(new EnemyFallState(Controller));
        }
    }

    public override void UpdatePhysics()
    {
        Controller.StopMovement();
    }
}
