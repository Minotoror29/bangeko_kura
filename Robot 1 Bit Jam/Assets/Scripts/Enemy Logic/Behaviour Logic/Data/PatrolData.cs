using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy Behaviour/Patrol")]
public class PatrolData : EnemyBehaviourData
{
    public override EnemyBehaviour Behaviour(EnemyController controller)
    {
        return new Patrol(controller);
    }
}
