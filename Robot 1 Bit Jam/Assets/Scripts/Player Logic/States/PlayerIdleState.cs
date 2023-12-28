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
        Rb.velocity = Vector2.zero;
    }

    public override void Exit()
    {
    }

    public override void OnCollisionEnter(Collision2D collision)
    {
    }

    public override void UpdateLogic()
    {
        if (Controls.InGame.Movement.ReadValue<Vector2>().magnitude > 0f)
        {
            Controller.ChangeState(new PlayerWalkState(Controller));
        }
    }

    public override void UpdatePhysics()
    {
    }
}
