using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasePlayerWhenClose : EnemyBehaviour
{
    private float _chasingDistance;
    private float _chasingTime;

    public ChasePlayerWhenClose(EnemyController controller, float chasingDistance, float chasingTime) : base(controller)
    {
        _chasingDistance = chasingDistance;
        _chasingTime = chasingTime;
    }

    public override void SubscribeEvents(EnemyState state)
    {
        state.OnUpdate += EnterChasingDistance;
    }

    public override void UnsubscribeEvents(EnemyState state)
    {
        state.OnUpdate -= EnterChasingDistance;
    }

    private void EnterChasingDistance()
    {
        if (Controller.DistanceToPlayer <= _chasingDistance)
        {
            Controller.ChangeState(new EnemyChasingState(Controller, _chasingDistance, _chasingTime));
        }
    }
}
