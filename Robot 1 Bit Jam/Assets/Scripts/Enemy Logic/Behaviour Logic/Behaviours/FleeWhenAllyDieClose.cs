using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleeWhenAllyDieClose : EnemyBehaviour
{
    public FleeWhenAllyDieClose(EnemyController controller) : base(controller)
    {
    }

    public override void SubscribeEvents(EnemyState state)
    {
        state.Controller.OnAllyDiedClose += Flee;
    }

    public override void UnsubscribeEvents(EnemyState state)
    {
        state.Controller.OnAllyDiedClose -= Flee;
    }

    private void Flee(Transform deathSource)
    {
        Controller.ChangeState(new EnemyFleeFromState(Controller, deathSource));
    }
}
