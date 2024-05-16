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

        FindPatrolDirection();

        _patrolTimer = Controller.PatrolTime;
    }

    public void FindPatrolDirection()
    {
        _direction = Random.insideUnitCircle;
        RaycastHit2D ray = Physics2D.Raycast(Controller.transform.position, _direction, _direction.magnitude, Controller.VoidLayer);
        if (ray.collider != null)
        {
            FindPatrolDirection();
        }
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

    public override void OnTriggerEnter(Collider2D collision)
    {
        base.OnTriggerEnter(collision);

        Controller.ChangeState(new EnemyIdleState(Controller));
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

        if (Controller.Grounds.Count == 0)
        {
            Controller.ChangeState(new EnemyFallState(Controller));
        }
    }

    public override void UpdatePhysics()
    {
        Controller.MoveTowards(_direction, Controller.MovementSpeed);
    }
}
