using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy Behaviour/Chase Player When Close")]
public class ChasePlayerWhenCloseData : EnemyBehaviourData
{
    public float chasingDistance;

    public override EnemyBehaviour Behaviour()
    {
        return new ChasePlayerWhenClose(chasingDistance);
    }
}
