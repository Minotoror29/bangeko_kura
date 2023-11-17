using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleeWhenAllyDieClose : EnemyBehaviour
{
    private float _fleeingTime;

    public FleeWhenAllyDieClose(EnemyController controller, float fleeingTime) : base(controller)
    {
        _fleeingTime = fleeingTime;
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
        Controller.ChangeState(new EnemyFleeFromState(Controller, deathSource, _fleeingTime));
    }
}
