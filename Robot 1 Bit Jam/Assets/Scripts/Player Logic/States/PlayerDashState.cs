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

        _dashOrigin = Controller.transform.position;

        if (_animationDirection == Direction.Forward)
        {
            Controller.Animator.CrossFade("Player Dash Forward", 0f);
        } else if (_animationDirection == Direction.Back)
        {
            Controller.Animator.CrossFade("Player Dash Back", 0f);
        } else if (_animationDirection == Direction.Right)
        {
            Controller.Animator.CrossFade("Player Dash Right", 0f);
        } else if (_animationDirection == Direction.Left)
        {
            Controller.Animator.CrossFade("Player Dash Left", 0f);
        }
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

    public override void UpdateLogic()
    {
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
