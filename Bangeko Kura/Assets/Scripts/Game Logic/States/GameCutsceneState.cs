using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCutsceneState : GameState
{
    public GameCutsceneState(GameManager gameManager) : base(gameManager)
    {
    }

    public override void Enter()
    {
        GameManager.CutsceneManager.StartCutscene(this);
    }

    public override void Exit()
    {
    }

    public void EndCutscene()
    {
        GameManager.ChangeState(new GamePlayState(GameManager));
    }

    public override void OnCollisionEnter(Collision2D collision)
    {
    }

    public override void OnCollisionStay(Collision2D collision)
    {
    }

    public override void UpdateLogic()
    {
        GameManager.CutsceneManager.UpdateLogic();
    }

    public override void UpdatePhysics()
    {
    }
}
