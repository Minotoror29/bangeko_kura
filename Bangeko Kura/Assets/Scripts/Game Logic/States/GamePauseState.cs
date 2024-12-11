using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePauseState : GameState
{
    private float _pauseTimer;

    public GamePauseState(GameManager gameManager, float pauseTime) : base(gameManager)
    {
        _pauseTimer = pauseTime;
    }

    public override void Enter()
    {
        GameManager.Player.Controls.Disable();
        GameManager.NegativeVolume.gameObject.SetActive(true);
    }

    public override void Exit()
    {
        GameManager.Player.Controls.Enable();
        GameManager.NegativeVolume.gameObject.SetActive(false);
    }

    public override void OnCollisionEnter(Collision2D collision)
    {
    }

    public override void OnCollisionStay(Collision2D collision)
    {
    }

    public override void UpdateLogic()
    {
        if (_pauseTimer > 0f)
        {
            _pauseTimer -= Time.deltaTime;

            if (_pauseTimer <= 0f)
            {
                GameManager.ChangeState(new GamePlayState(GameManager));
            }
        }
    }

    public override void UpdatePhysics()
    {
    }
}
