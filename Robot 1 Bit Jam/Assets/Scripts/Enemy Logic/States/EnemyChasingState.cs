using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChasingState : EnemyState
{
    public EnemyChasingState(EnemyController controller) : base(controller)
    {
        Id = EnemyStateId.Chasing;
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

        Controller.LookTowards(Controller.Player.transform.position - Controller.transform.position);
    }

    public override void UpdatePhysics()
    {
        Controller.MoveTowards(Controller.Player.transform.position - Controller.transform.position);
    }
}
