using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeathState : PlayerState
{
    public PlayerDeathState(NewPlayerController controller) : base(controller)
    {
    }

    public override void Enter()
    {
        Controller.Die(Controller.HealthSystem, null);
    }

    public override void Exit()
    {
    }

    public override void OnCollisionEnter(Collision2D collision)
    {
    }

    public override void UpdateLogic()
    {
    }

    public override void UpdatePhysics()
    {
    }
}
