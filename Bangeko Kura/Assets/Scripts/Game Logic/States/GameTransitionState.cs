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
        _timer = Camera.main.GetComponent<CinemachineBrain>().m_DefaultBlend.m_Time;

        GameManager.Player.Rb.velocity = Vector2.zero;
        GameManager.Player.MeshAnimator.speed = 0;
    }

    public override void Exit()
    {
        GameManager.Player.MeshAnimator.speed = 1;
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
            GameManager.ChangeState(new GamePlayState(GameManager));
        }
    }

    public override void UpdatePhysics()
    {
    }
}
