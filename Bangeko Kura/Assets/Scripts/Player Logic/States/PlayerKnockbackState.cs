using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKnockbackState : PlayerState
{
    private Vector2 _direction;
    private float _distance;
    private float _speed;
    private Vector2 _origin;

    public PlayerKnockbackState(NewPlayerController controller, Vector2 direction, Knockback knockback) : base(controller)
    {
        _direction = direction;
        _distance = knockback.knockbackDistance;
        _speed = knockback.knockbackSpeed;
    }

    public override void Enter()
    {
        _origin = Controller.transform.position;
    }

    public override void Exit()
    {
    }

    public override bool CanBeKnockbacked()
    {
        return false;
    }

    public override void OnCollisionEnter(Collision2D collision)
    {
    }

    public override void OnCollisionStay(Collision2D collision)
    {
    }

    public override void UpdateLogic()
    {
        if (((Vector2)Controller.transform.position - _origin).magnitude >= _distance)
        {
            Controller.ChangeState(new PlayerIdleState(Controller));
        }
    }

    public override void UpdatePhysics()
    {
        Controller.Move(_direction, _speed);
    }
}
