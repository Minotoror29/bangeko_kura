using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLandState : PlayerState
{
    private Vector2 _spawnPosition;
    private float _landTimer = 1.333f;

    private bool _landEffectSpawned = false;

    public PlayerLandState(NewPlayerController controller, Vector2 spawnPosition, GameObject ground) : base(controller)
    {
        _spawnPosition = spawnPosition;
        Controller.AddGround(ground);
    }

    public override void Enter()
    {
        Controller.LandMesh.transform.position = _spawnPosition;
        Controller.LandMesh.SetActive(true);
    }

    public override void Exit()
    {
        Controller.LandMesh.SetActive(false);

        Controller.transform.position = _spawnPosition;

        Controller.Mesh.gameObject.SetActive(true);
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
            Controller.ChangeState(new PlayerIdleState(Controller));
        }

        if (_landTimer < 0.75f)
        {
            if (!_landEffectSpawned)
            {
                Controller.InstantiateEffect(Controller.LandEffect, _spawnPosition, Quaternion.identity, Controller.LandEffectLifetime);
                _landEffectSpawned = true;

                List<Collider2D> results = new();
                ContactFilter2D contactFilter = new() { useTriggers = true, layerMask = Controller.HealthSystemLayer };
                Physics2D.OverlapCircle(_spawnPosition, Controller.LandDamageRadius, contactFilter, results);
                foreach (Collider2D collider in results)
                {
                    if (collider.TryGetComponent(out HealthSystem hs))
                    {
                        if (hs.Source != Controller.transform)
                        {
                            hs.TakeDamage(Controller.LandDamage, Controller.transform);
                        }
                    }
                    
                }
            }
        }

        if (_landTimer < 0.633f)
        {
            Controls.InGame.Enable();
            if (Controls.InGame.Movement.ReadValue<Vector2>().magnitude > 0f)
            {
                Controller.ChangeState(new PlayerWalkState(Controller));
            }
        }
    }

    public override void UpdatePhysics()
    {
    }
}
