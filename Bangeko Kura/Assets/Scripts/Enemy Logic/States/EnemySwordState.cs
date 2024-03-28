using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySwordState : EnemyState
{
    private float _buildupTimer;
    private bool _buildingUp = false;
    private float _timer = 0.958f;

    public EnemySwordState(EnemyController controller, float buildupTime) : base(controller)
    {
        Id = EnemyStateId.Sword;

        _buildupTimer = buildupTime;
    }

    public override void Enter()
    {
        base.Enter();

        _buildingUp = true;
        Controller.MeshAnimator.CrossFade("Enemy Sword Buildup", 0f);
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

        if (_buildupTimer > 0f)
        {
            _buildupTimer -= Time.deltaTime;
        } else
        {
            if (_buildingUp)
            {
                Controller.MeshAnimator.CrossFade("Enemy Sword", 0f);
                _buildingUp = false;
            }

            if (_timer > 0f)
            {
                _timer -= Time.deltaTime;
            }
            else
            {
                Controller.ChangeState(new EnemyIdleState(Controller));
            }
        }        
    }

    public override void UpdatePhysics()
    {
        Controller.StopMovement();
    }
}
