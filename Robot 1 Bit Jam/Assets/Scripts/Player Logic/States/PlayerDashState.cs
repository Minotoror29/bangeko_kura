using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerState
{
    private Vector2 _dashDirection;
    private Vector2 _dashOrigin;

    public PlayerDashState(NewPlayerController controller, Vector2 dashDirection) : base(controller)
    {
        _dashDirection = dashDirection;
    }

    public override void Enter()
    {
        _dashOrigin = Controller.transform.position;
    }

    public override void Exit()
    {
    }

    public override void OnCollisionEnter(Collision2D collision)
    {
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
}
