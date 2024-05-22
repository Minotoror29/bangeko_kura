using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallState : PlayerState
{
    private float _fallTimer = 0.4f;

    public PlayerFallState(PlayerController controller) : base(controller)
    {
    }

    public override void Enter()
    {
        Controller.SetCollidersActive(false);
        Controls.InGame.Disable();
        Controller.Mesh.gameObject.SetActive(false);

        if (Rb.velocity.y < 0)
        {
            Controller.FallDownSprite.transform.position = (Vector2)Controller.Mesh.position;
            Controller.FallDownSprite.SetActive(true);
        } else
        {
            Controller.FallSprite.transform.position = (Vector2)Controller.Mesh.position;
            Controller.FallSprite.SetActive(true);
        }

        Controller.FallingSound.start();
    }

    public override void Exit()
    {
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
        if (_fallTimer > 0f)
        {
            _fallTimer -= Time.deltaTime;

            if (_fallTimer <= 0f)
            {
                Controller.FallDownSprite.SetActive(false);
                Controller.FallSprite.SetActive(false);
                Controller.HealthSystem.TakeDamageFromFall(3);
                if (Controller.HealthSystem.CurrentHealth > 0)
                {
                    Controller.Mesh.gameObject.SetActive(false);
                    Controller.GameManager.PlayerFell();
                }
            }
        }
    }

    public override void UpdatePhysics()
    {
        Rb.velocity = Vector2.zero;
    }
}
