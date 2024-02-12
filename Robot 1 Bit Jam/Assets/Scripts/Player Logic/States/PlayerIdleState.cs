using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerState
{
    private Vector2 _lookDirection;
    private Direction _currentState;
    private float _turnTimer;

    public PlayerIdleState(NewPlayerController controller) : base(controller)
    {
    }

    public override void Enter()
    {
        Controller.Animator.CrossFade("Player Idle", 0.5f);

        _lookDirection = Camera.main.ScreenToWorldPoint(Controls.InGame.MousePosition.ReadValue<Vector2>()) - Controller.transform.position;
        _lookDirection.Normalize();

        if (_lookDirection.y > 0.71f)
        {
            _currentState = Direction.Forward;
        }
        else if (_lookDirection.y < -0.71f)
        {
            _currentState = Direction.Back;
        }
        else if (_lookDirection.x > 0.71f)
        {
            _currentState = Direction.Right;
        }
        else if (_lookDirection.x < -0.71f)
        {
            _currentState = Direction.Left;
        }

        ChangeRotation();
    }

    private void ChangeRotation()
    {
        if (_currentState == Direction.Forward)
        {
            Quaternion meshRotation = Quaternion.LookRotation(Vector3.up, Controller.Mesh.up);
            Controller.Mesh.localRotation = Quaternion.Euler(new Vector3(0f, meshRotation.eulerAngles.y, 0f));
        }
        else if (_currentState == Direction.Back)
        {
            Quaternion meshRotation = Quaternion.LookRotation(Vector3.down, Controller.Mesh.up);
            Controller.Mesh.localRotation = Quaternion.Euler(new Vector3(0f, meshRotation.eulerAngles.y, 0f));
        }
        else if (_currentState == Direction.Right)
        {
            Quaternion meshRotation = Quaternion.LookRotation(Vector3.right, Controller.Mesh.up);
            Controller.Mesh.localRotation = Quaternion.Euler(new Vector3(0f, meshRotation.eulerAngles.y, 0f));
        }
        else if (_currentState == Direction.Left)
        {
            Quaternion meshRotation = Quaternion.LookRotation(Vector3.left, Controller.Mesh.up);
            Controller.Mesh.localRotation = Quaternion.Euler(new Vector3(0f, meshRotation.eulerAngles.y, 0f));
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
        _lookDirection = Camera.main.ScreenToWorldPoint(Controls.InGame.MousePosition.ReadValue<Vector2>()) - Controller.transform.position;
        _lookDirection.Normalize();

        if (_lookDirection.y > 0.71f)
        {
            if (_currentState != Direction.Forward)
            {
                if (_currentState == Direction.Right)
                {
                    Animator.CrossFade("Player Turn Left", 0f);
                    _turnTimer = 0.533f;
                } else if (_currentState == Direction.Left)
                {
                    Animator.CrossFade("Player Turn Right", 0f);
                    _turnTimer = 0.533f;
                }
                _currentState = Direction.Forward;
            }
        }
        else if (_lookDirection.y < -0.71f)
        {
            if (_currentState != Direction.Back)
            {
                if (_currentState == Direction.Right)
                {
                    Animator.CrossFade("Player Turn Right", 0f);
                    _turnTimer = 0.533f;
                }
                else if (_currentState == Direction.Left)
                {
                    Animator.CrossFade("Player Turn Left", 0f);
                    _turnTimer = 0.533f;
                }
                _currentState = Direction.Back;
            }
        }
        else if (_lookDirection.x > 0.71f)
        {
            if (_currentState != Direction.Right)
            {
                if (_currentState == Direction.Forward)
                {
                    Animator.CrossFade("Player Turn Right", 0f);
                    _turnTimer = 0.533f;
                }
                else if (_currentState == Direction.Back)
                {
                    Animator.CrossFade("Player Turn Left", 0f);
                    _turnTimer = 0.533f;
                }
                _currentState = Direction.Right;
            }
        }
        else if (_lookDirection.x < -0.71f)
        {
            if (_currentState != Direction.Left)
            {
                if (_currentState == Direction.Forward)
                {
                    Animator.CrossFade("Player Turn Left", 0f);
                    _turnTimer = 0.533f;
                }
                else if (_currentState == Direction.Back)
                {
                    Animator.CrossFade("Player Turn Right", 0f);
                    _turnTimer = 0.533f;
                }
                _currentState = Direction.Left;
            }
        }

        if (_turnTimer > 0f)
        {
            _turnTimer -= Time.deltaTime;

            if (_turnTimer <= 0f)
            {
                Animator.CrossFade("Player Idle", 0.2f);

                ChangeRotation();
            }
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
