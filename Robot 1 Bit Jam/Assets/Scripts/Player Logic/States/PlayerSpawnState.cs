using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnState : PlayerState
{
    private float _spawnTimer;
    private Vector2 _spawnPosition;

    public PlayerSpawnState(NewPlayerController controller, Vector2 position) : base(controller)
    {
        _spawnPosition = position;
    }

    public override void Enter()
    {
        Controller.transform.position = _spawnPosition;
        Controls.InGame.Enable();
        Controller.Mesh.gameObject.SetActive(true);
        _spawnTimer = 0.1f;

        Animator.CrossFade("Player Idle", 0f);
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
