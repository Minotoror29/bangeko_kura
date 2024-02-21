using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerState
{
    public PlayerIdleState(NewPlayerController controller) : base(controller)
    {
    }

    public override void Enter()
    {
        Controller.OnDash += Dash;

        Controller.Animator.CrossFade("Player Idle", 0.1f);
    }

    public override void Exit()
    {
        Controller.OnDash -= Dash;
    }

    public override void OnCollisionEnter(Collision2D collision)
    {
    }

    private void Dash()
    {
        Controller.ChangeState(new PlayerDashState(Controller, Controller.LookDirection.normalized, Direction.Forward));
    }

    public override void UpdateLogic()
    {
        Controller.RotateAim();

        if (Controls.InGame.Movement.ReadValue<Vector2>().magnitude > 0f)
        {
            Controller.ChangeState(new PlayerWalkState(Controller));
        }

        if (Controller.Grounds.Count == 0)
        {
            Controller.ChangeState(new PlayerFallState(Controller));
        }
    }

    public override void UpdatePhysics()
    {
        Rb.velocity = Vector2.zero;
    }
}
