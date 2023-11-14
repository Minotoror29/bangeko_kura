using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleeWhenAllyDieClose : EnemyBehaviour
{
    public override void SubscribeEvents(EnemyState state)
    {
        state.Controller.OnAllyDiedClose += Flee;
    }

    public override void UnsubscribeEvents(EnemyState state)
    {
        state.Controller.OnAllyDiedClose -= Flee;
    }

    private void Flee(EnemyController controller, Transform deathSource)
    {
        controller.ChangeState(new EnemyFleeFromState(controller, deathSource));
    }
}
