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
        Controls.InGame.Disable();
        
        if (Controller.HealthSystem.CurrentHealth > 0)
        {
            Controller.Mesh.gameObject.SetActive(false);
            Controller.ScreenManager.PlayerDied();
        } else
        {
            Animator.CrossFade("Player Death", 0f);
        }
    }

    public override void Exit()
    {
    }

    public override void OnCollisionEnter(Collision2D collision)
    {
    }

    public override void OnCollisionStay(Collision2D collision)
    {
    }

    public override void UpdateLogic()
    {
    }

    public override void UpdatePhysics()
    {
    }
}
