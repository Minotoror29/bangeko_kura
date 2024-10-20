using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTransitionState : GameState
{
    private float _timer;

    public GameTransitionState(GameManager gameManager) : base(gameManager)
    {
    }

    public override void Enter()
    {
        GameManager.Player.Controls.Disable();
        _timer = Camera.main.GetComponent<CinemachineBrain>().m_DefaultBlend.m_Time;
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
        if (_timer > 0f)
        {
            _timer -= Time.deltaTime;
        } else
        {
            GameManager.Player.Controls.Enable();
            GameManager.ChangeState(new GamePlayState(GameManager));
        }
    }

    public override void UpdatePhysics()
    {
    }
}
