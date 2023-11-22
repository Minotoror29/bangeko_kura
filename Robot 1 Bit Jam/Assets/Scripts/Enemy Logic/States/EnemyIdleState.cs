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
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void OnCollisionEnter(Collision2D collision)
    {
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
    }

    public override void UpdatePhysics()
    {
        Controller.StopMovement();
    }
}
