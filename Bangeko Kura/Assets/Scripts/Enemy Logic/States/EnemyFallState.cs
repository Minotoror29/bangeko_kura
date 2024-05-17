using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFallState : EnemyState
{
    private float _fallTimer = 0.4f;

    public EnemyFallState(EnemyController controller) : base(controller)
    {
    }

    public override void Enter()
    {
        base.Enter();

        Controller.SetCollidersActive(false);
        Controller.Mesh.gameObject.SetActive(false);

        Controller.FallSprite.transform.position = (Vector2)Controller.Mesh.position + Vector2.up + Vector2.right * 0.5f;
        Controller.FallSprite.SetActive(true);
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

        if (_fallTimer > 0f)
        {
            _fallTimer -= Time.deltaTime;

            if (_fallTimer <= 0f)
            {
                Controller.FallSprite.SetActive(false);
                Controller.EnemiesManager.RemoveEnemy(Controller);
                Controller.gameObject.SetActive(false);
            }
        }
    }

    public override void UpdatePhysics()
    {
    }
}
