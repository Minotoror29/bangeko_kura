using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class PlayerDashState : PlayerState
{
    PlayerWalkController _controller;

    private Vector2 _dashDirection;
    private Vector2 _dashOrigin;
    private Direction _animationDirection;

    public PlayerDashState(PlayerController controller, Vector2 dashDirection, Direction animationDirection) : base(controller)
    {
        _controller = controller as PlayerWalkController;
        _dashDirection = dashDirection;
        _animationDirection = animationDirection;
    }

    public override void Enter()
    {
        Controller.OnTakeDamage += PreventDamage;

        Controller.RotateMesh();

        _dashOrigin = Controller.transform.position;

        if (_animationDirection == Direction.Forward)
        {
            Controller.MeshAnimator.CrossFade("Player Dash Forward", 0f);
        } else if (_animationDirection == Direction.Back)
        {
            Controller.MeshAnimator.CrossFade("Player Dash Back", 0f);
        } else if (_animationDirection == Direction.Right)
        {
            Controller.MeshAnimator.CrossFade("Player Dash Right", 0f);
        } else if (_animationDirection == Direction.Left)
        {
            Controller.MeshAnimator.CrossFade("Player Dash Left", 0f);
        }

        Controller.InstantiateEffect(_controller.DashEffect, Controller.transform.position, Quaternion.LookRotation(Vector3.forward, _dashDirection.normalized));
        foreach (ParticleSystem particle in _controller.DashParticles)
        {
            ParticleSystem.EmissionModule emission = particle.emission;
            emission.enabled = true;
        }

        _controller.DashSound.start();
    }

    public override void Exit()
    {
        Controller.OnTakeDamage -= PreventDamage;

        foreach (ParticleSystem particle in _controller.DashParticles)
        {
            ParticleSystem.EmissionModule emission = particle.emission;
            emission.enabled = false;
        }
    }

    public override bool CanBeKnockbacked()
    {
        return false;
    }

    public override void OnCollisionEnter(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Controller.ChangeState(new PlayerIdleState(Controller));
        }
    }

    public override void OnCollisionStay(Collision2D collision)
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

    public override void UpdateLogic()
    {
        foreach (ParticleSystem particle in _controller.DashParticles)
        {
            if (!particle.isPlaying)
            {
                particle.Play();
            }
        }

        Controller.UpdateWeapons();

        if (((Vector2)Controller.transform.position - _dashOrigin).magnitude >= _controller.DashDistance)
        {
            Controller.ChangeState(new PlayerIdleState(Controller));
        }
    }

    public override void UpdatePhysics()
    {
        Controller.Move(_dashDirection, _controller.DashSpeed);
    }

    private void PreventDamage()
    {
        Controller.HealthSystem.PreventDamage = true;
    }
}
