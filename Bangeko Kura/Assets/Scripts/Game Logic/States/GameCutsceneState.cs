using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCutsceneState : GameState
{
    private CutsceneManager _cutsceneManager;

    public GameCutsceneState(GameManager gameManager, CutsceneManager cutsceneManager) : base(gameManager)
    {
        _cutsceneManager = cutsceneManager;
    }

    public override void Enter()
    {
        Cursor.visible = false;
        GameManager.GameCanvas.gameObject.SetActive(false);
        GameManager.Player.Controls.Disable();

        _cutsceneManager.StartCutscene();
    }

    public override void Exit()
    {
        Cursor.visible = true;
        GameManager.GameCanvas.gameObject.SetActive(true);
    }

    public void EndCutscene()
    {
        GameManager.Player.Controls.Enable();
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
        _cutsceneManager.UpdateLogic();
    }

    public override void UpdatePhysics()
    {
    }
}
