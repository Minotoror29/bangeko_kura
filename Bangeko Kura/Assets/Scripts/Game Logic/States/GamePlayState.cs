using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayState : GameState
{
    public GamePlayState(GameManager gameManager) : base(gameManager)
    {
    }

    public override void Enter()
    {
        GameManager.Player.MeshAnimator.speed = 1;
        GameManager.Player.HealthSystem.GetComponent<CapsuleCollider2D>().enabled = true;
    }

    public override void Exit()
    {
        GameManager.Player.Rb.velocity = Vector2.zero;
        GameManager.Player.MeshAnimator.speed = 0;
        GameManager.CurrentScreen.PauseScreen();
        GameManager.Player.HealthSystem.GetComponent<CapsuleCollider2D>().enabled = false;
    }

    public override void OnCollisionEnter(Collision2D collision)
    {
    }

    public override void OnCollisionStay(Collision2D collision)
    {
    }

    public override void UpdateLogic()
    {
        GameManager.Player.UpdateLogic();
        GameManager.CurrentScreen.UpdateLogic();
    }

    public override void UpdatePhysics()
    {
        GameManager.Player.UpdatePhysics();
        GameManager.CurrentScreen.UpdatePhysics();
    }
}
