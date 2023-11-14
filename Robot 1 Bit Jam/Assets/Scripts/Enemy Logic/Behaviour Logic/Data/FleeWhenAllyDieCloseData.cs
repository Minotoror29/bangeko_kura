using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy Behaviour/Flees When Ally Dies Close")]
public class FleeWhenAllyDieCloseData : EnemyBehaviourData
{
    public override EnemyBehaviour Behaviour()
    {
        return new FleeWhenAllyDieClose();
    }
}
