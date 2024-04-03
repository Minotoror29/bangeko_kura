using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyKnockBackState : EnemyState
{
    private Vector2 _direction;
    private float _knockBackTimer = 0.2f;

    public EnemyKnockBackState(EnemyController controller, Vector2 direction) : base(controller)
    {
        Id = EnemyStateId.KnockBack;

        _direction = direction;
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

        if (_knockBackTimer > 0f)
        {
            _knockBackTimer -= Time.deltaTime;
        } else
        {
            Controller.ChangeState(new EnemyIdleState(Controller));
        }
    }

    public override void UpdatePhysics()
    {
        Controller.MoveTowards(_direction, 100f);
    }
}
