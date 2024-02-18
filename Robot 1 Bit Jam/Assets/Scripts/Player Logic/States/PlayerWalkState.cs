using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction { Forward, Back, Right, Left }

public class PlayerWalkState : PlayerState
{
    private Vector2 _walkAnimDirection;
    private Direction _currentState;

    public PlayerWalkState(NewPlayerController controller) : base(controller)
    {
    }

    public override void Enter()
    {
        Controller.OnDash += Dash;

        _currentState = Direction.Forward;
        Animator.CrossFade("Player Walk Forward", 0.1f);
    }

    public override void Exit()
    {
        Controller.OnDash -= Dash;
    }

    public override void OnCollisionEnter(Collision2D collision)
    {
    }

    public override void UpdateLogic()
    {
        _walkAnimDirection = new Vector2(Controller.Mesh.transform.InverseTransformDirection(Controls.InGame.Movement.ReadValue<Vector2>()).x, Controller.Mesh.transform.InverseTransformDirection(Controls.InGame.Movement.ReadValue<Vector2>()).z).normalized;

        if (_walkAnimDirection.y > 0.71f)
        {
            if (_currentState != Direction.Forward)
            {
                _currentState = Direction.Forward;
                Animator.CrossFade("Player Walk Forward", 0.25f);
            }
        } else if (_walkAnimDirection.y < -0.71f)
        {
            if (_currentState != Direction.Back)
            {
                _currentState = Direction.Back;
                Animator.CrossFade("Player Walk Back", 0.5f);
            }
        } else if (_walkAnimDirection.x > 0.71f)
        {
            if (_currentState != Direction.Right)
            {
                _currentState = Direction.Right;
                Animator.CrossFade("Player Walk Right", 0.5f);
            }
        } else if (_walkAnimDirection.x < -0.71f)
        {
            if (_currentState != Direction.Left)
            {
                _currentState = Direction.Left;
                Animator.CrossFade("Player Walk Left", 0.5f);
            }
        }

        if (Controls.InGame.Movement.ReadValue<Vector2>().magnitude == 0f)
        {
            Controller.ChangeState(new PlayerIdleState(Controller));
        }

        if (Controller.Grounds.Count == 0)
        {
            Controller.Die(Controller.HealthSystem, null);
        }
    }

    public override void UpdatePhysics()
    {
        Controller.Move(Controls.InGame.Movement.ReadValue<Vector2>(), Controller.MovementSpeed);
    }

    private void Dash()
    {
        Controller.ChangeState(new PlayerDashState(Controller, Controls.InGame.Movement.ReadValue<Vector2>()));
    }
}
