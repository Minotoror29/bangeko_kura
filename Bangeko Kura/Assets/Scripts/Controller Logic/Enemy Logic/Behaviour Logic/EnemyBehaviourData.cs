using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBehaviourData : ScriptableObject
{
    public List<EnemyStateId> states;

    public abstract EnemyBehaviour Behaviour(EnemyController controller);
}
