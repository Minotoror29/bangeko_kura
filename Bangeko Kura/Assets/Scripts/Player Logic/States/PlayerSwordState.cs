using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwordState : PlayerState
{
    private float _effectTimer = 0.2f;
    private float _timer = 0.633f;

    public PlayerSwordState(NewPlayerController controller) : base(controller)
    {
    }

    public override void Enter()
    {
        Controller.OnDash += Dash;

        Animator.CrossFade("Player Sword", 0f);
    }

    public override void Exit()
    {
        Controller.OnDash -= Dash;
    }

    public override bool CanBeKnockbacked()
    {
        return false;
    }

    public override void OnCollisionEnter(Collision2D collision)
    {
    }

    public override void OnCollisionStay(Collision2D collision)
    {
    }

    public override void OnTriggerEnter(Collider2D collision)
    {
    }

    private void Dash()
    {
        if (_effectTimer > 0f) return;

        if (Controls.InGame.Movement.ReadValue<Vector2>() == Vector2.zero)
        {
            Controller.ChangeState(new PlayerDashState(Controller, Controller.LookDirection.normalized, Direction.Forward));
        }
        else
        {
            Controller.ChangeState(new PlayerDashState(Controller, Controls.InGame.Movement.ReadValue<Vector2>(), Direction.Forward));
        }
    }

    public override void UpdateLogic()
    {
        Controller.Turret.UpdateLogic();
        Controller.Sword.UpdateLogic();

        Controller.RotateMesh();

        if (_effectTimer > 0f)
        {
            _effectTimer -= Time.deltaTime;
        }

        if (_timer > 0f)
        {
            _timer -= Time.deltaTime;
        } else
        {
            Controller.ChangeState(new PlayerIdleState(Controller));
        }

        if (Controller.Grounds.Count == 0)
        {
            Controller.ChangeState(new PlayerFallState(Controller));
        }
    }

    public override void UpdatePhysics()
    {
        Controller.Move(Controls.InGame.Movement.ReadValue<Vector2>(), Controller.MovementSpeed);
    }
}
