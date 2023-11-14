using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy Behaviour/Chase Player When Close")]
public class ChasePlayerWhenClose : EnemyBehaviourData
{
    public float chasingDistance;

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
        if (controller.DistanceToPlayer <= chasingDistance)
        {
            controller.ChangeState(new EnemyChasingState(controller));
        }
    }
}
