using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyKnockBackState : EnemyState
{
    private Vector2 _direction;
    private float _distance;
    private float _speed;
    private Vector2 _origin;

    public EnemyKnockBackState(EnemyController controller, Vector2 direction, Knockback knockback) : base(controller)
    {
        Id = EnemyStateId.KnockBack;

        _direction = direction;
        _distance = knockback.knockbackDistance;
        _speed = knockback.knockbackSpeed;
    }

    public override void Enter()
    {
        base.Enter();

        _origin = Controller.transform.position;
    }

    public override void OnCollisionEnter(Collision2D collision)
    {
    }

    public override void OnCollisionStay(Collision2D collision)
    {
    }

    public override bool CanBeKnockedBack()
    {
        return false;
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        if (((Vector2)Controller.transform.position - _origin).magnitude >= _distance)
        {
            Controller.ChangeState(new EnemyIdleState(Controller));
        }
    }

    public override void UpdatePhysics()
    {
        Controller.MoveTowards(_direction, _speed);
    }
}
