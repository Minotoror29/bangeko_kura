using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHeadController : PlayerController
{
    [Space]
    [SerializeField] private float laserKnockbackPlayerDistance;
    [SerializeField] private float laserKnockbackPlayerSpeed;
    [SerializeField] private List<ParticleSystem> knockbackParticles;

    private Knockback _laserPlayerKnockback;

    public override void Initialize(GameManager gameManager)
    {
        base.Initialize(gameManager);

        _laserPlayerKnockback = new Knockback { knockbackDistance= laserKnockbackPlayerDistance, knockbackSpeed = laserKnockbackPlayerSpeed };

        foreach (ParticleSystem particle in knockbackParticles)
        {
            ParticleSystem.EmissionModule emission = particle.emission;
            emission.enabled = false;
        }
    }

    public override void FireLaser()
    {
        if (LaserCooldownTimer > 0f) return;

        ChangeState(new PlayerKnockbackState(this, -LookDirection, _laserPlayerKnockback, knockbackParticles));

        base.FireLaser();
    }

    public override void PlayParticles(bool play)
    {
        base.PlayParticles(play);

        foreach (ParticleSystem particle in knockbackParticles)
        {
            if (play)
            {
                particle.Play();
            } else
            {
                particle.Pause();
            }
        }
    }

    public override void Move(Vector2 direction, float speed)
    {
        base.Move(direction, speed);

        Mesh.Rotate(new Vector2(direction.y, -direction.x) * (speed / 10), Space.World);
    }

    public override void RotateMesh()
    {
    }

    public override void RotateMeshSmooth(Vector3 direction, float speed)
    {
    }
}
