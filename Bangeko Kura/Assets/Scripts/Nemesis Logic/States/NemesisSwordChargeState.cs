using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NemesisSwordChargeState : NemesisState
{
    private float _chargeTimer;

    private Action OnSwordEnd;

    public NemesisSwordChargeState(NemesisPhase phase, Action onSwordEnd) : base(phase)
    {
        OnSwordEnd = onSwordEnd;
    }

    public override void Enter()
    {
        _chargeTimer = Controller.SwordChargeTime;
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
        if (_chargeTimer > 0f)
        {
            _chargeTimer -= Time.deltaTime;
        } else
        {
            Phase.ChangeState(new NemesisSwordAttackState(Phase, OnSwordEnd));
        }
    }

    public override void UpdatePhysics()
    {
        Rb.velocity = Vector2.zero;
    }
}
