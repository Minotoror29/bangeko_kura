using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : EnemyState
{
    private float _time;
    private float _timer;

    public EnemyIdleState(EnemyController controller, float time) : base(controller)
    {
        _time = time;
    }

    public override void Enter()
    {
        _timer = 0f;
    }

    public override void Exit()
    {
    }

    public override void UpdateLogic()
    {
        Controller.StopMovement();

        if (_time > 0f)
        {
            if (_timer < _time)
            {
                _timer += Time.deltaTime;
            } else
            {
                Controller.ChangeState(new EnemyPatrolState(Controller));
            }
        }
    }

    public override void UpdatePhysics()
    {
    }
}
