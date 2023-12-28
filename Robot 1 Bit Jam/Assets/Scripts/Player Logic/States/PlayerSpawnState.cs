using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnState : PlayerState
{
    private float _spawnTimer;

    public PlayerSpawnState(NewPlayerController controller) : base(controller)
    {
    }

    public override void Enter()
    {
        _spawnTimer = 0.1f;
    }

    public override void Exit()
    {
    }

    public override void OnCollisionEnter(Collision2D collision)
    {
    }

    public override void UpdateLogic()
    {
        if (_spawnTimer > 0f)
        {
            _spawnTimer -= Time.deltaTime;
        } else
        {
            Controller.ChangeState(new PlayerIdleState(Controller));
        }
    }

    public override void UpdatePhysics()
    {
    }
}
