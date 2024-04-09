using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeathState : PlayerState
{
    private bool _fromFall;

    public PlayerDeathState(NewPlayerController controller, bool fromFall) : base(controller)
    {
        _fromFall = fromFall;
    }

    public override void Enter()
    {
        Controller.SetCollidersActive(false);
        Controls.InGame.Disable();
        
        if (!_fromFall)
        {
            Animator.CrossFade("Player Death", 0f);
        }
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

    public override void OnTriggerEnter(Collider2D collision)
    {
    }

    public override void UpdateLogic()
    {
    }

    public override void UpdatePhysics()
    {
    }
}
