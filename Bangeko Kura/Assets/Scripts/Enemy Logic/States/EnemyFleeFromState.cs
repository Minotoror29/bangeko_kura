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

        Controller.MeshAnimator.CrossFade("Enemy Walk", 0f);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void OnTriggerEnter(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Controller.ChangeState(new EnemyIdleState(Controller));
        }
    }

    public override void OnCollisionStay(Collision2D collision)
    {
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        Controller.LookTowards(Controller.Rb.velocity, false);

        _fleeingTimer -= Time.deltaTime;
        if (_fleeingTimer <= 0 )
        {
            Controller.ChangeState(new EnemyIdleState(Controller));
        }

        if (Controller.Grounds.Count == 0)
        {
            Controller.ChangeState(new EnemyFallState(Controller));
        }
    }

    public override void UpdatePhysics()
    {
        Controller.MoveTowards(Controller.transform.position - _target.position, Controller.MovementSpeed);
    }

    public override void OnCollisionEnter(Collision2D collision)
    {
    }
}
