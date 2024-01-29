using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NemesisShootState : NemesisState
{
    private float _shootTimer;
    private int _projectilesToShoot;

    private event Action OnShootEnd;

    public NemesisShootState(NemesisPhase phase, Action onShootEnd) : base(phase)
    {
        OnShootEnd += onShootEnd;
    }

    public override void Enter()
    {
        Animator.CrossFade("Player Idle", 0f);
        _shootTimer = Phase.Data.shootTime;
        _projectilesToShoot = UnityEngine.Random.Range(Phase.Data.minProjectilesPerSalvo, Phase.Data.maxProjectilesPerSalvo + 1);
    }

    public override void Exit()
    {
    }

    public override void OnCollisionEnter(Collision2D collision)
    {
    }

    public override void UpdateLogic()
    {
        if (_shootTimer > 0f)
        {
            _shootTimer -= Time.deltaTime;
        } else
        {
            Controller.ShootBullet();
            _projectilesToShoot--;

            if (_projectilesToShoot == 0)
            {
                OnShootEnd?.Invoke();
            } else
            {
                _shootTimer = Phase.Data.shootTime;
            }
        }
    }

    public override void UpdatePhysics()
    {
        Rb.velocity = Vector2.zero;
    }
}
