using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallState : PlayerState
{
    private float _fallTimer = 0.4f;

    public PlayerFallState(NewPlayerController controller) : base(controller)
    {
    }

    public override void Enter()
    {
        Controller.Mesh.gameObject.SetActive(false);
        if (Rb.velocity.y < 0)
        {
            Controller.FallDownSprite.SetActive(true);
            Controller.FallDownSprite.transform.position = (Vector2)Controller.Mesh.position + Vector2.up + Vector2.right * 0.5f/* + Rb.velocity.normalized * 0.5f*/;
        } else
        {
            Controller.FallSprite.SetActive(true);
            Controller.FallSprite.transform.position = (Vector2)Controller.Mesh.position + Vector2.up + Vector2.right * 0.5f/* + Rb.velocity.normalized * 0.5f*/;
        }
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
        if (_fallTimer > 0f)
        {
            _fallTimer -= Time.deltaTime;
        } else
        {
            Controller.FallDownSprite.SetActive(false);
            Controller.FallSprite.SetActive(false);
            Controller.ChangeState(new PlayerDeathState(Controller));
        }
    }

    public override void UpdatePhysics()
    {
        Rb.velocity = Vector2.zero;
        //Rb.velocity = new Vector2(0f , -_fallSpeed) * Time.fixedDeltaTime;
    }
}
