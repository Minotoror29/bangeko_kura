using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBehaviour
{
    private EnemyController _controller;

    public EnemyController Controller { get { return _controller; } }

    public EnemyBehaviour(EnemyController controller)
    {
        _controller = controller;
    }

    public abstract void SubscribeEvents(EnemyState state);

    public abstract void UnsubscribeEvents(EnemyState state);
}
