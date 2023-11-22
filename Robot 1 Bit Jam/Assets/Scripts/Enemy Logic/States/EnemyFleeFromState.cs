using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFleeFromState : EnemyState
{
    private Transform _target;

    private float _fleeingTimer;

    public EnemyFleeFromState(EnemyController controller, Transform target, float fleeingTime) : base(controller)
    {
        Id = EnemyStateId.Fleeing;
        _target = target;
        _fleeingTimer = fleeingTime;
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
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Controller.ChangeState(new EnemyIdleState(Controller));
        }
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        _fleeingTimer -= Time.deltaTime;
        if (_fleeingTimer <= 0 )
        {
            Controller.ChangeState(new EnemyIdleState(Controller));
        }
    }

    public override void UpdatePhysics()
    {
        Controller.MoveTowards(Controller.transform.position - _target.position);
    }
}
