using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RotationDirection { Static, Left, Right };

public class PlayerIdleState : PlayerState
{
    private RotationDirection _currentRotationDirection;

    private float _waitBeforeidleTime = 0.5f;
    private float _waitBeforeIdleTimer;

    public PlayerIdleState(NewPlayerController controller) : base(controller)
    {
    }

    public override void Enter()
    {
        Controller.Animator.CrossFade("Player Idle", 0.5f);

        _currentRotationDirection = RotationDirection.Static;
    }

    public override void Exit()
    {
    }

    public override void OnCollisionEnter(Collision2D collision)
    {
    }

    public override void UpdateLogic()
    {
        Controller.RotateSmooth();

        if (_waitBeforeIdleTimer > 0f && _currentRotationDirection == RotationDirection.Static)
        {
            _waitBeforeIdleTimer -= Time.deltaTime;
        }

        if (Controller.RotationDirection != _currentRotationDirection)
        {
            _currentRotationDirection = Controller.RotationDirection;

            if (_currentRotationDirection == RotationDirection.Static)
            {
                _waitBeforeIdleTimer = _waitBeforeidleTime;
            } else if (_currentRotationDirection == RotationDirection.Left)
            {
                Controller.Animator.CrossFade("Player Turn Left", 0.25f);
            } else if (_currentRotationDirection == RotationDirection.Right)
            {
                Controller.Animator.CrossFade("Player Turn Right", 0.25f);
            }
        }

        if (_currentRotationDirection == RotationDirection.Static && _waitBeforeIdleTimer <= 0f)
        {
            Controller.Animator.CrossFade("Player Idle", 0.25f);
        }

        if (Controls.InGame.Movement.ReadValue<Vector2>().magnitude > 0f)
        {
            Controller.ChangeState(new PlayerWalkState(Controller));
        }

        if (Controller.Grounds.Count == 0)
        {
            Controller.Die(Controller.HealthSystem, null);
        }
    }

    public override void UpdatePhysics()
    {
        Rb.velocity = Vector2.zero;
    }
}
