using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum IdleState { Static, RotatingSlow, RotatingMedium, RotatingFast, FinishingRotation }

public class PlayerIdleState : PlayerState
{
    private IdleState _currentState;
    private Vector2 _bodyDirection;
    private float _angle;
    private Vector2 _targetDirection;
    private float _slowRotationSeed = 250f;
    private float _mediumRotationSeed = 500f;
    private float _fastRotationSeed = 1000f;

    private float _bufferTimer = 0.1f;

    public PlayerIdleState(NewPlayerController controller) : base(controller)
    {
    }

    public override void Enter()
    {
        Controller.OnDash += Dash;

        _currentState = IdleState.Static;
        Controller.Animator.CrossFade("Player Idle", 0.1f);
    }

    public override void Exit()
    {
        Controller.OnDash -= Dash;
    }

    public override void OnCollisionEnter(Collision2D collision)
    {
    }

    private void Dash()
    {
        Controller.ChangeState(new PlayerDashState(Controller, Controller.LookDirection.normalized, Direction.Forward));
    }

    public override void UpdateLogic()
    {
        foreach (Weapon weapon in Controller.Weapons)
        {
            weapon.UpdateLogic();
        }

        _bodyDirection = new Vector2((Quaternion.AngleAxis(45, Vector3.right) * Controller.Mesh.forward).x, (Quaternion.AngleAxis(45, Vector3.right) * Controller.Mesh.forward).z);
        _angle = Vector2.Angle(_bodyDirection, Controller.LookDirection.normalized);

        if (_angle > 135f)
        {
            if (_currentState == IdleState.Static || _currentState == IdleState.FinishingRotation || _currentState == IdleState.RotatingSlow || _currentState == IdleState.RotatingMedium)
            {
                _targetDirection = Controller.LookDirection.normalized;
                Animator.CrossFade("Player Turn 180", 0.1f);
                _currentState = IdleState.RotatingFast;
            }
        }
        else if (_angle > 90f)
        {
            if (_currentState == IdleState.Static || _currentState == IdleState.FinishingRotation || _currentState == IdleState.RotatingSlow)
            {
                _targetDirection = Controller.LookDirection.normalized;
                Animator.CrossFade("Player Turn Left", 0.1f);
                _currentState = IdleState.RotatingMedium;
            }
        }
        else if (_angle > 45f)
        {
            if (_currentState == IdleState.Static || _currentState == IdleState.FinishingRotation)
            {
                _targetDirection = Controller.LookDirection.normalized;
                Animator.CrossFade("Player Fast Turn Left", 0.1f);
                _currentState = IdleState.RotatingSlow;
            }
        } 

        if (_currentState == IdleState.RotatingSlow)
        {
            Controller.RotateMeshSmooth(_targetDirection, _slowRotationSeed);
        } else if (_currentState == IdleState.RotatingMedium)
        {
            Controller.RotateMeshSmooth(_targetDirection, _mediumRotationSeed);
        } else if (_currentState == IdleState.RotatingFast)
        {
            Controller.RotateMeshSmooth(_targetDirection, _fastRotationSeed);
        }

        if (_currentState != IdleState.Static && _currentState != IdleState.FinishingRotation)
        {
            if (Vector2.Angle(_bodyDirection, _targetDirection) == 0f)
            {
                _bufferTimer = 0.1f;
                _currentState = IdleState.FinishingRotation;
            }
        }

        if (_currentState == IdleState.FinishingRotation)
        {
            if (_bufferTimer > 0f)
            {
                _bufferTimer -= Time.deltaTime;
            } else
            {
                Animator.CrossFade("Player Idle", 0.1f);
                _currentState = IdleState.Static;
            }
        }

        Controller.RotateAim();

        if (Controls.InGame.Movement.ReadValue<Vector2>().magnitude > 0f)
        {
            Controller.ChangeState(new PlayerWalkState(Controller));
        }

        if (Controller.Grounds.Count == 0)
        {
            Controller.ChangeState(new PlayerFallState(Controller));
        }
    }

    public override void UpdatePhysics()
    {
        Rb.velocity = Vector2.zero;
    }
}
