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
        GameManager.Player.UpdateLogic();
    }

    public override void UpdatePhysics()
    {
        GameManager.Player.UpdatePhysics();
    }
}
