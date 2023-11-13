using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFleeFromState : EnemyState
{
    private Transform _target;

    private float _fleeingTimer;

    public EnemyFleeFromState(EnemyController controller, Transform target) : base(controller)
    {
        _target = target;
    }

    public override void Enter()
    {
        Debug.Log(Controller.gameObject.name + " is fleeing");

        _fleeingTimer = 5f;
    }

    public override void Exit()
    {
    }

    public override void UpdateLogic()
    {
        _fleeingTimer -= Time.deltaTime;
        if (_fleeingTimer <= 0 )
        {
            Controller.ChangeState(new EnemyIdleState(Controller, Controller.GetRandomIdleTime()));
        }
    }

    public override void UpdatePhysics()
    {
        Controller.MoveTowards(Controller.transform.position - _target.position);
    }
}
