using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NemesisDashState : NemesisState
{
    private int _dashNumber;

    private Vector2 _dashDirection;
    private Vector2 _dashOrigin;

    private event Action<int> OnDashEnd;
    private Action OnStunEnd;

    public NemesisDashState(NemesisPhase phase, int dashNumber, Action<int> onDashEnd, Action onStunEnd) : base(phase)
    {
        _dashNumber = dashNumber;

        OnDashEnd += onDashEnd;
        OnStunEnd = onStunEnd;
    }

    public override void Enter()
    {
        _dashDirection = Player.transform.position - Controller.transform.position;
        _dashOrigin = Controller.transform.position;

        Animator.CrossFade("Player Dash", 0f);
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
            OnDashEnd?.Invoke(_dashNumber);
        }
    }

    public override void UpdatePhysics()
    {
        Controller.MoveTowards(_dashDirection, Controller.DashSpeed);
    }

    public override void TakeDamage()
    {
        base.TakeDamage();

        Phase.ChangeState(new NemesisStunState(Phase, OnStunEnd));
    }
}
