using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLandState : PlayerState
{
    private Vector2 _spawnPosition;
    private float _landTimer = 1.467f;

    public PlayerLandState(NewPlayerController controller, Vector2 spawnPosition) : base(controller)
    {
        _spawnPosition = spawnPosition;
    }

    public override void Enter()
    {
        Controller.LandMesh.transform.position = _spawnPosition;
        Controller.LandMesh.SetActive(true);
    }

    public override void Exit()
    {
        Controller.LandMesh.SetActive(false);
    }

    public override void OnCollisionEnter(Collision2D collision)
    {
    }

    public override void UpdateLogic()
    {
        if (_landTimer > 0f)
        {
            _landTimer -= Time.deltaTime;
        } else
        {
            Controller.ChangeState(new PlayerSpawnState(Controller, _spawnPosition));
        }
    }

    public override void UpdatePhysics()
    {
    }
}
