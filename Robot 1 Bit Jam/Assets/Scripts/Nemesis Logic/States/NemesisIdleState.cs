using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NemesisIdleState : NemesisState
{
    private event Action OnUpdate;

    public NemesisIdleState(NemesisPhase phase, Action onUpdateAction) : base(phase)
    {
        OnUpdate += onUpdateAction;
    }

    public override void Enter()
    {
        Animator.CrossFade("Player Idle", 0f);
    }

    public override void Exit()
    {
    }

    public override void OnCollisionEnter(Collision2D collision)
    {
    }

    public override void UpdateLogic()
    {
        OnUpdate?.Invoke();
    }

    public override void UpdatePhysics()
    {
        Rb.velocity = Vector2.zero;
    }
}
