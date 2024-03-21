using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy Behaviour/Chase Player When Close")]
public class ChasePlayerWhenCloseData : EnemyBehaviourData
{
    public float chasingDistance;
    public float chasingTime;

    public override EnemyBehaviour Behaviour(EnemyController controller)
    {
        return new ChasePlayerWhenClose(controller, chasingDistance, chasingTime);
    }
}
