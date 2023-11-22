using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrolState : EnemyState
{
    private Vector3 _direction;
    private float _patrolTimer;

    public EnemyPatrolState(EnemyController controller) : base(controller)
    {
        Id = EnemyStateId.Patrol;
    }

    public override void Enter()
    {
        base.Enter();

        //_direction = Random.insideUnitSphere;
        //_direction.y = 0f;

        _direction = Random.insideUnitCircle;

        _patrolTimer = Controller.PatrolTime;
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

        _patrolTimer -= Time.deltaTime;

        if (_patrolTimer <= 0f)
        {
            Controller.ChangeState(new EnemyIdleState(Controller));
        }
    }

    public override void UpdatePhysics()
    {
        Controller.MoveTowards(_direction);
    }
}
