using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NemesisWalkState : NemesisState
{
    private float _walkTimer;

    private event Action OnWalkEnd;

    public NemesisWalkState(NemesisPhase phase, float walkTime, Action onWalkEnd) : base(phase)
    {
        _walkTimer = walkTime;

        OnWalkEnd += onWalkEnd;
    }

    public override void Enter()
    {
        Animator.CrossFade("Player Walk", 0f);
    }

    public override void Exit()
    {
    }

    public override void OnCollisionEnter(Collision2D collision)
    {
    }

    public override void UpdateLogic()
    {
        if (_walkTimer > 0f)
        {
            _walkTimer -= Time.deltaTime;
        } else
        {
            OnWalkEnd?.Invoke();
        }
    }

    public override void UpdatePhysics()
    {
        Controller.MoveTowards(Player.transform.position - Controller.transform.position, Controller.WalkSpeed);
    }

    public override void OnCollisionStay(Collision2D collision)
    {
    }
}