using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NemesisStunState : NemesisState
{
    private float _stunTimer;

    public NemesisStunState(NemesisPhase phase) : base(phase)
    {
    }

    public override void Enter()
    {
        Animator.CrossFade("Player Idle", 0f);
        _stunTimer = Controller.StunTime;
    }

    public override void Exit()
    {
    }

    public override void OnCollisionEnter(Collision2D collision)
    {
    }

    public override void UpdateLogic()
    {
        if (_stunTimer > 0f)
        {
            _stunTimer -= Time.deltaTime;
        } else
        {
            Phase.ChangeState(new NemesisIdleState(Phase));
        }
    }

    public override void UpdatePhysics()
    {
        Rb.velocity = Vector2.zero;
    }
}
