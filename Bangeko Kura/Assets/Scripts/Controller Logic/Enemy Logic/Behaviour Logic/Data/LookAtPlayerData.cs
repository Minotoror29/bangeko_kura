using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy Behaviour/Look At Player")]
public class LookAtPlayerData : EnemyBehaviourData
{
    public override EnemyBehaviour Behaviour(EnemyController controller)
    {
        return new LookAtPlayer(controller);
    }
}
