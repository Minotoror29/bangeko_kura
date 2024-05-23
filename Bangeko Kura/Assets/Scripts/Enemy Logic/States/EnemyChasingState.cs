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

        Controller.MeshAnimator.CrossFade("Enemy Walk", 0f);
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

    public override bool CanAttackSword(float buildupTime)
    {
        Controller.ChangeState(new EnemySwordState(Controller, buildupTime));

        return true;
    }

    public override bool CanDash(float dashTime, float dashSpeed, Vector2 dashDirection)
    {
        Controller.ChangeState(new EnemyDashState(Controller, dashTime, dashSpeed, dashDirection));

        return true;
    }

    public override void PlayerDied()
    {
        base.PlayerDied();

        Controller.ChangeState(new EnemyIdleState(Controller));
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        Controller.LookTowards(Controller.Player.transform.position - Controller.transform.position, false);

        //if (_chasingTimer < _chasingTime)
        //{
        //    _chasingTimer += Time.deltaTime;
        //} else
        //{
        //    if ((Controller.Player.transform.position - Controller.transform.position).magnitude > _chasingDistance)
        //    {
        //        Controller.ChangeState(new EnemyIdleState(Controller));
        //    }
        //}

        if (Controller.Grounds.Count == 0)
        {
            Controller.ChangeState(new EnemyFallState(Controller));
        }
    }

    public override void UpdatePhysics()
    {
        Controller.MoveTowards(Controller.Player.transform.position - Controller.transform.position, Controller.MovementSpeed);
    }
}
