using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction { Forward, Back, Right, Left }

public class PlayerWalkState : PlayerState
{
    private Vector2 _walkAnimDirection;
    private Direction _currentDirection;

    public PlayerWalkState(PlayerController controller) : base(controller)
    {
    }

    public override void Enter()
    {
        _currentDirection = Direction.Forward;
        Animator.CrossFade("Player Walk Forward", 0.1f);
    }

    public override void Exit()
    {
    }

    public override void OnCollisionEnter(Collision2D collision)
    {
    }

    public override void OnTriggerEnter(Collider2D collision)
    {
        if (collision.TryGetComponent(out Elevator elevator))
        {
            if (elevator.CurrentState == ElevatorState.Waiting) return;

            Controller.ChangeState(new PlayerMoveToElevatorState(Controller, elevator));
        }
    }

    public override bool CanAttackSword()
    {
        Controller.ChangeState(new PlayerSwordState(Controller));

        return true;
    }

    public override void UpdateLogic()
    {
        Controller.UpdateWeapons();

        Controller.RotateMesh();
        Controller.RotateAim();

        _walkAnimDirection = new Vector2(Controller.Mesh.transform.InverseTransformDirection(Controls.InGame.Movement.ReadValue<Vector2>()).x, Controller.Mesh.transform.InverseTransformDirection(Controls.InGame.Movement.ReadValue<Vector2>()).z).normalized;

        if (_walkAnimDirection.y > 0.71f)
        {
            if (_currentDirection != Direction.Forward)
            {
                _currentDirection = Direction.Forward;
                Animator.CrossFade("Player Walk Forward", 0.25f);
            }
        } else if (_walkAnimDirection.y < -0.71f)
        {
            if (_currentDirection != Direction.Back)
            {
                _currentDirection = Direction.Back;
                Animator.CrossFade("Player Walk Back", 0.5f);
            }
        } else if (_walkAnimDirection.x > 0.71f)
        {
            if (_currentDirection != Direction.Right)
            {
                _currentDirection = Direction.Right;
                Animator.CrossFade("Player Walk Right", 0.5f);
            }
        } else if (_walkAnimDirection.x < -0.71f)
        {
            if (_currentDirection != Direction.Left)
            {
                _currentDirection = Direction.Left;
                Animator.CrossFade("Player Walk Left", 0.5f);
            }
        }

        if (Controls.InGame.Movement.ReadValue<Vector2>().magnitude == 0f)
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

    public override bool CanDash()
    {
        Controller.ChangeState(new PlayerDashState(Controller, Controls.InGame.Movement.ReadValue<Vector2>(), _currentDirection));

        return true;
    }

    public override void OnCollisionStay(Collision2D collision)
    {
    }
}
