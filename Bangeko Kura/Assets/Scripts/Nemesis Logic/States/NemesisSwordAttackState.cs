using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NemesisSwordAttackState : NemesisState
{
    private float _attackTimer = 0.458f;

    private event Action OnSwordEnd;

    public NemesisSwordAttackState(NemesisPhase phase, Action onSwordEnd) : base(phase)
    {
        OnSwordEnd += onSwordEnd;
    }

    public override void Enter()
    {
        Animator.CrossFade("Player Sword", 0f);
        if ((Player.transform.position - Controller.transform.position).magnitude <= Controller.SwordDistance)
        {
            //Player.HealthSystem.TakeDamage(Controller.SwordDamage, Controller.transform);
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
            OnSwordEnd?.Invoke();
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
