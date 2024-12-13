using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrolState : EnemyState
{
    private Vector2 _direction;
    private float _patrolTimer;
    private ParticleSystem.EmissionModule _emission;

    public EnemyPatrolState(EnemyController controller) : base(controller)
    {
        Id = EnemyStateId.Patrol;
        _emission = Controller.WalkParticles.emission;
    }

    public override void Enter()
    {
        base.Enter();

        Controller.MeshAnimator.CrossFade("Enemy Walk", 0f);

        FindPatrolDirection();

        _patrolTimer = Controller.PatrolTime;

        _emission.enabled = true;
    }

    public void FindPatrolDirection()
    {
        _direction = Random.insideUnitCircle;
    }

    public override void Exit()
    {
        base.Exit();

        _emission.enabled = false;
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
