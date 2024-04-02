using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwordState : PlayerState
{
    private float _timer = 0.633f;

    public PlayerSwordState(NewPlayerController controller) : base(controller)
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

    public override void OnCollisionStay(Collision2D collision)
    {
    }

    public override void UpdateLogic()
    {
        Controller.Turret.UpdateLogic();
        Controller.Sword.UpdateLogic();

        Controller.RotateMesh();

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
