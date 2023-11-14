using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBehaviour
{
    public abstract void SubscribeEvents(EnemyState state);

    public abstract void UnsubscribeEvents(EnemyState state);
}
