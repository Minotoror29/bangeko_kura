using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBehaviourData : ScriptableObject
{
    public List<EnemyStateId> states;

    public abstract void SubscribeEvents(EnemyState state);

    public abstract void UnsubscribeEvents(EnemyState state);
}

public enum EnemyStateId { Idle, Patrol, Fleeing, Chasing }
