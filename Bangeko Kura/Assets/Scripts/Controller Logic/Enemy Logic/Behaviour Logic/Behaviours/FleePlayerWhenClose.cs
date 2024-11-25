using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleePlayerWhenClose : EnemyBehaviour
{
    private float _fleeingDistance;
    private float _fleeingTime;

    public FleePlayerWhenClose(EnemyController controller, float fleeingDistance, float fleeingTime) : base(controller)
    {
        _fleeingDistance = fleeingDistance;
        _fleeingTime = fleeingTime;
    }

    public override void SubscribeEvents(EnemyState state)
    {
        state.OnUpdate += CheckDistanceToPlayer;
    }

    public override void UnsubscribeEvents(EnemyState state)
    {
        state.OnUpdate -= CheckDistanceToPlayer;
    }

    private void CheckDistanceToPlayer()
    {
        if (Controller.DistanceToPlayer <= _fleeingDistance)
        {
            Controller.ChangeState(new EnemyFleeFromState(Controller, Controller.Player.transform, _fleeingTime));
        }
    }
}
