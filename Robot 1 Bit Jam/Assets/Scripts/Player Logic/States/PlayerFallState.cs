using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallState : PlayerState
{
    private float _fallTimer = 1f;
    //private float _fallSpeed = 250f;

    public PlayerFallState(NewPlayerController controller) : base(controller)
    {
    }

    public override void Enter()
    {
        //Controller.Mesh.gameObject.SetActive(false);
        //Controller.FallMesh.gameObject.SetActive(true);
        //Controller.FallMesh.Initialize(Controller.Mesh.position, Controller.Mesh.localRotation);
        Animator.CrossFade("Player Fall", 0.1f);
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
            //Controller.FallMesh.gameObject.SetActive(false);
            Controller.ChangeState(new PlayerDeathState(Controller));
        }
    }

    public override void UpdatePhysics()
    {
        Rb.velocity = Vector2.zero;
        //Rb.velocity = new Vector2(0f , -_fallSpeed) * Time.fixedDeltaTime;
    }
}
