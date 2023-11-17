using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy Behaviour/Flee Player When Close")]
public class FleePlayerWhenCloseData : EnemyBehaviourData
{
    public float fleeingDistance;
    public float fleeingTime;

    public override EnemyBehaviour Behaviour(EnemyController controller)
    {
        return new FleePlayerWhenClose(controller, fleeingDistance, fleeingTime);
    }
}
