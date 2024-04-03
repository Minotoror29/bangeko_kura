using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrolState : EnemyState
{
    private Vector2 _direction;
    private float _patrolTimer;

    public EnemyPatrolState(EnemyController controller) : base(controller)
    {
        Id = EnemyStateId.Patrol;
    }

    public override void Enter()
    {
        base.Enter();

        Controller.MeshAnimator.CrossFade("Enemy Walk", 0f);

        _direction = Random.insideUnitCircle;

        _patrolTimer = Controller.PatrolTime;
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
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            _direction += collision.GetContact(0).normal;
        }
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        _patrolTimer -= Time.deltaTime;

        Controller.LookTowards(Controller.Rb.velocity, false);

        if (_patrolTimer <= 0f)
        {
            Controller.ChangeState(new EnemyIdleState(Controller));
        }
    }

    public override void UpdatePhysics()
    {
        Controller.MoveTowards(_direction, Controller.MovementSpeed);
    }
}
