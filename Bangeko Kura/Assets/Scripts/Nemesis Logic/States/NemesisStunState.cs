using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NemesisStunState : NemesisState
{
    private float _stunTimer;

    private event Action OnStunEnd;

    public NemesisStunState(NemesisPhase phase, Action onStunEnd) : base(phase)
    {
        OnStunEnd += onStunEnd;
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
            OnStunEnd?.Invoke();
        }
    }

    public override void UpdatePhysics()
    {
        Rb.velocity = Vector2.zero;
    }

    public override void OnCollisionStay(Collision2D collision)
    {
    }
}
