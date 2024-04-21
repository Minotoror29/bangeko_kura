using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLandState : EnemyState
{
    private Vector2 _spawnPosition;

    public EnemyLandState(EnemyController controller) : base(controller)
    {
    }

    public override void Enter()
    {
        base.Enter();

        _spawnPosition = Controller.transform.position;
        Controller.transform.position = _spawnPosition + Vector2.up * 25f;
        Controller.SetCollidersActive(false);
    }

    public override void Exit()
    {
        base.Exit();

        Controller.SetCollidersActive(true);
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

        Controller.transform.position = Vector2.MoveTowards(Controller.transform.position, _spawnPosition, Controller.LandSpeed * Time.deltaTime);

        if ((Vector2)Controller.transform.position == _spawnPosition)
        {
            List<Collider2D> results = new();
            ContactFilter2D contactFilter = new() { useTriggers = true, layerMask = Controller.HealthSystemLayer };
            Physics2D.OverlapCircle(_spawnPosition, Controller.LandDamageRadius, contactFilter, results);
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

            Controller.ChangeState(new EnemyIdleState(Controller));
        }
    }

    public override void UpdatePhysics()
    {
    }
}
