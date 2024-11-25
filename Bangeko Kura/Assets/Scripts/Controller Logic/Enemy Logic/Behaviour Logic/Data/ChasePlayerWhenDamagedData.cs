using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy Behaviour/Chase Player When Damaged")]
public class ChasePlayerWhenDamagedData : EnemyBehaviourData
{
    public override EnemyBehaviour Behaviour(EnemyController controller)
    {
        return new ChasePlayerWhenDamaged(controller);
    }
}
