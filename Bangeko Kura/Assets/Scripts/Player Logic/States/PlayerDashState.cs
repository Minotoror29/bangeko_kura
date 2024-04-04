using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class PlayerDashState : PlayerState
{
    private Vector2 _dashDirection;
    private Vector2 _dashOrigin;
    private Direction _animationDirection;

    public PlayerDashState(NewPlayerController controller, Vector2 dashDirection, Direction animationDirection) : base(controller)
    {
        _dashDirection = dashDirection;
        _animationDirection = animationDirection;
    }

    public override void Enter()
    {
        Controller.OnTakeDamage += PreventDamage;

        Controller.RotateMesh();

        _dashOrigin = Controller.transform.position;

        if (_animationDirection == Direction.Forward)
        {
            Controller.MeshAnimator.CrossFade("Player Dash Forward", 0f);
        } else if (_animationDirection == Direction.Back)
        {
            Controller.MeshAnimator.CrossFade("Player Dash Back", 0f);
        } else if (_animationDirection == Direction.Right)
        {
            Controller.MeshAnimator.CrossFade("Player Dash Right", 0f);
        } else if (_animationDirection == Direction.Left)
        {
            Controller.MeshAnimator.CrossFade("Player Dash Left", 0f);
        }

        Controller.InstantiateEffect(Controller.DashEffect, Controller.transform.position, Quaternion.LookRotation(Vector3.forward, _dashDirection.normalized), 0.367f);

        Controller.DashSound.start();
    }

    public override void Exit()
    {
        Controller.OnTakeDamage -= PreventDamage;
    }

    public override void OnCollisionEnter(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Controller.ChangeState(new PlayerIdleState(Controller));
        }
    }

    public override void OnCollisionStay(Collision2D collision)
    {
    }

    public override void UpdateLogic()
    {
        Controller.Turret.UpdateLogic();
        Controller.Sword.UpdateLogic();

        if (((Vector2)Controller.transform.position - _dashOrigin).magnitude >= Controller.DashDistance)
        {
            Controller.ChangeState(new PlayerIdleState(Controller));
        }
    }

    public override void UpdatePhysics()
    {
        Controller.Move(_dashDirection, Controller.DashSpeed);
    }

    private void PreventDamage()
    {
        Controller.HealthSystem.PreventDamage = true;
    }
}
