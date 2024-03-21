using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy Behaviour/Flees When Ally Dies Close")]
public class FleeWhenAllyDieCloseData : EnemyBehaviourData
{
    public float fleeingTime;

    public override EnemyBehaviour Behaviour(EnemyController controller)
    {
        return new FleeWhenAllyDieClose(controller, fleeingTime);
    }
}
