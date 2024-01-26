using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NemesisSwordAttackState : NemesisState
{
    private float _attackTimer = 0.458f;

    public NemesisSwordAttackState(NemesisPhase phase) : base(phase)
    {
    }

    public override void Enter()
    {
        Animator.CrossFade("Player Sword", 0f);
        if ((Player.transform.position - Controller.transform.position).magnitude <= Controller.WalkDistance)
        {
            Player.HealthSystem.TakeDamage(Controller.SwordDamage, Controller.transform);
        }
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
            Controller.SwordCooldownTimer = Controller.SwordCooldown;
            Phase.ChangeState(new NemesisIdleState(Phase));
        }
    }

    public override void UpdatePhysics()
    {
        Rb.velocity = Vector2.zero;
    }
}
