using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalkState : PlayerState
{
    public PlayerWalkState(NewPlayerController controller) : base(controller)
    {
    }

    public override void Enter()
    {
        Controller.OnDash += Dash;

        Animator.CrossFade("Player Walk", 0f);
    }

    public override void Exit()
    {
        Controller.OnDash -= Dash;
    }

    public override void OnCollisionEnter(Collision2D collision)
    {
    }

    public override void UpdateLogic()
    {
        if (Controls.InGame.Movement.ReadValue<Vector2>().magnitude == 0f)
        {
            Controller.ChangeState(new PlayerIdleState(Controller));
        }

        if (Controller.Grounds.Count == 0)
        {
            Controller.Die(Controller.HealthSystem, null);
        }
    }

    public override void UpdatePhysics()
    {
        Controller.Move(Controls.InGame.Movement.ReadValue<Vector2>(), Controller.MovementSpeed);
    }

    private void Dash()
    {
        Controller.ChangeState(new PlayerDashState(Controller, Controls.InGame.Movement.ReadValue<Vector2>()));
    }
}
