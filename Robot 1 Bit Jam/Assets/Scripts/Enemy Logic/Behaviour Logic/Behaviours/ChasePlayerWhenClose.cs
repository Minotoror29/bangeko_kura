using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasePlayerWhenClose : EnemyBehaviour
{
    private float _chasingDistance;

    public ChasePlayerWhenClose(float chasingDistance)
    {
        _chasingDistance = chasingDistance;
    }

    public override void SubscribeEvents(EnemyState state)
    {
        state.OnUpdate += EnterChasingDistance;
    }

    public override void UnsubscribeEvents(EnemyState state)
    {
        state.OnUpdate -= EnterChasingDistance;
    }

    private void EnterChasingDistance(EnemyController controller)
    {
        if (controller.DistanceToPlayer <= _chasingDistance)
        {
            controller.ChangeState(new EnemyChasingState(controller));
        }
    }
}
