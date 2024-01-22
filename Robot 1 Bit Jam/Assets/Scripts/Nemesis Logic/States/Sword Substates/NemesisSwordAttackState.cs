using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NemesisSwordAttackState : NemesisSwordSubstate
{
    private float _attackTimer = 0.458f;

    public NemesisSwordAttackState(NemesisController controller, NemesisSwordState superstate) : base(controller, superstate)
    {
    }

    public override void Enter()
    {
        Animator.CrossFade("Player Sword", 0f);
    }

    public override void Exit()
    {
    }

    public override void OnCollisionEnter(Collision2D collision)
    {
    }

    public override void UpdateLogic()
    {
        if (_attackTimer > 0f)
        {
            _attackTimer -= Time.deltaTime;
        } else
        {
            Superstate.CheckNextState();
        }
    }

    public override void UpdatePhysics()
    {
    }
}
