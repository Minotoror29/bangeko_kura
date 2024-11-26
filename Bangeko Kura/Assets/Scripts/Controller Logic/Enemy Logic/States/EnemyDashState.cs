using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDashState : EnemyState
{
    private float _timer;
    private float _dashSpeed;
    private Vector2 _dashDirection;

    public EnemyDashState(EnemyController controller, float dashTime, float dashSpeed, Vector2 dashDirection) : base(controller)
    {
        Id = EnemyStateId.Dash;

        _timer = dashTime;
        _dashSpeed = dashSpeed;
        _dashDirection = dashDirection;
    }

    public override void Enter()
    {
        base.Enter();

        Controller.MeshAnimator.CrossFade("Enemy Dash", 0f);
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

        if (_timer > 0)
        {
            _timer -= Time.deltaTime;
        } else
        {
            Controller.ChangeState(new EnemyIdleState(Controller));
        }
    }

    public override void UpdatePhysics()
    {
        Controller.Rb.velocity = _dashSpeed * Time.fixedDeltaTime * _dashDirection.normalized;
    }
}
