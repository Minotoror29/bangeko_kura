using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLandState : EnemyState
{
    private GameObject _shadow;
    private float _landTimer = 1.167f;
    private float _landDamageTime = 1f;
    private bool _landed = false;

    public EnemyLandState(EnemyController controller, GameObject ground) : base(controller)
    {
        Controller.AddGround(ground);
    }

    public override void Enter()
    {
        base.Enter();

        Controller.Mesh.gameObject.SetActive(false);
        Controller.SetCollidersActive(false);
        _shadow = Controller.InstantiateEffect(Controller.ShadowPrefab, Controller.transform.position, Quaternion.identity, 2f);
        Controller.LandMesh.transform.position = Controller.transform.position;
        Controller.LandMesh.SetActive(true);
    }

    public override void Exit()
    {
        base.Exit();

        Controller.LandMesh.SetActive(false);

        Controller.SetCollidersActive(true);
        Controller.Mesh.gameObject.SetActive(true);
    }

    public override bool CanBeKnockedBack()
    {
        return false;
    }

    public override void OnCollisionEnter(Collision2D collision)
    {
    }

    public override void OnCollisionStay(Collision2D collision)
    {
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        if (_landTimer > 0f)
        {
            _landTimer -= Time.deltaTime;
        }
        else
        {
            Controller.ChangeState(new EnemyIdleState(Controller));
        }

        _shadow.transform.localScale = Vector2.one * ((1.167f- _landTimer) / .167f);

        if (_landTimer < _landDamageTime)
        {
            if (!_landed)
            {
                _landed = true;

                List<Collider2D> results = new();
                ContactFilter2D contactFilter = new() { useTriggers = true, layerMask = Controller.HealthSystemLayer };
                Physics2D.OverlapCircle(Controller.transform.position, Controller.LandDamageRadius, contactFilter, results);
                foreach (Collider2D collider in results)
                {
                    if (collider.TryGetComponent(out HealthSystem hs))
                    {
                        if (hs.CompareTag(collider.tag))
                        {
                            hs.TakeDamage(Controller.LandDamage, Controller.transform, Controller.LandKnockback);
                        }
                    }
                }

                _shadow.SetActive(false);
            }
        }
    }

    public override void UpdatePhysics()
    {
    }
}
