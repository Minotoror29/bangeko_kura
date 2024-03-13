using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallState : PlayerState
{
    private float _fallTimer = 0.367f;

    public PlayerFallState(NewPlayerController controller) : base(controller)
    {
    }

    public override void Enter()
    {
        Controller.Mesh.gameObject.SetActive(false);
        Controller.FallSprite.SetActive(true);
        Controller.FallSprite.transform.position = (Vector2)Controller.Mesh.position + Vector2.up;
    }

    public override void Exit()
    {
    }

    public override void OnCollisionEnter(Collision2D collision)
    {
    }

    public override void UpdateLogic()
    {
        if (_fallTimer > 0f)
        {
            _fallTimer -= Time.deltaTime;
        } else
        {
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
