using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasePlayerWhenClose : EnemyBehaviour
{
    private float _chasingDistance;

    public ChasePlayerWhenClose(EnemyController controller, float chasingDistance) : base(controller)
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

    private void EnterChasingDistance()
    {
        if (Controller.DistanceToPlayer <= _chasingDistance)
        {
            Controller.ChangeState(new EnemyChasingState(Controller));
        }
    }
}
