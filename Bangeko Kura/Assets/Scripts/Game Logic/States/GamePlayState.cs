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

        foreach (BulletController bullet in GameManager.Bullets)
        {
            bullet.Rb.velocity = Vector2.zero;
        }
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

        GameManager.CloudsManager.UpdateLogic();

        for (int i = GameManager.Bullets.Count - 1; i >= 0; i--)
        {
            if (GameManager.Bullets[i].gameObject.activeSelf)
            {
                GameManager.Bullets[i].UpdateLogic();
            }
            else
            {
                GameManager.Bullets[i].DestroyBullet();
                GameManager.Bullets.RemoveAt(i);
            }
        }
    }

    public override void UpdatePhysics()
    {
        GameManager.Player.UpdatePhysics();
        GameManager.CurrentScreen.UpdatePhysics();

        foreach (BulletController bullet in GameManager.Bullets)
        {
            bullet.UpdatePhysics();
        }
    }
}
