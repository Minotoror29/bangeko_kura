using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySwordState : EnemyState
{
    private float _timer = 0.958f;

    public EnemySwordState(EnemyController controller) : base(controller)
    {
    }

    public override void Enter()
    {
        base.Enter();

        Controller.MeshAnimator.CrossFade("Enemy Sword", 0f);
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

        if (_timer > 0f)
        {
            _timer -= Time.deltaTime;
        }
        else
        {
            Controller.ChangeState(new EnemyIdleState(Controller));
        }
    }

    public override void UpdatePhysics()
    {
        Controller.StopMovement();
    }
}
