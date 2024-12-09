using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKnockbackState : PlayerState
{
    private Vector2 _direction;
    private float _distance;
    private float _speed;
    private Vector2 _origin;
    private List<ParticleSystem> _particles;

    public PlayerKnockbackState(PlayerController controller, Vector2 direction, Knockback knockback) : base(controller)
    {
        _direction = direction;
        _distance = knockback.knockbackDistance;
        _speed = knockback.knockbackSpeed;
    }

    public PlayerKnockbackState(PlayerController controller, Vector2 direction, Knockback knockback, List<ParticleSystem> knockbackParticles) : base(controller)
    {
        _direction = direction;
        _distance = knockback.knockbackDistance;
        _speed = knockback.knockbackSpeed;
        _particles = knockbackParticles;
    }

    public override void Enter()
    {
        _origin = Controller.transform.position;

        if (_particles != null)
        {
            foreach (ParticleSystem particle in _particles)
            {
                ParticleSystem.EmissionModule emission = particle.emission;
                emission.enabled = true;
            }
        }
    }

    public override void Exit()
    {
        if (_particles != null)
        {
            foreach (ParticleSystem particle in _particles)
            {
                ParticleSystem.EmissionModule emission = particle.emission;
                emission.enabled = false;
            }
        }
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

    public override void UpdateLogic()
    {
        if (_particles != null)
        {
            foreach (ParticleSystem particle in _particles)
            {
                if (!particle.isPlaying)
                {
                    particle.Play();
                }
            }
        }

        if (((Vector2)Controller.transform.position - _origin).magnitude >= _distance)
        {
            Controller.ChangeState(new PlayerIdleState(Controller));
        }
    }

    public override void UpdatePhysics()
    {
        Controller.Move(_direction.normalized, _speed);
    }
}
