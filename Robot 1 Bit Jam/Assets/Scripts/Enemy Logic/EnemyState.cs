using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyState : State
{
    private EnemyController _controller;

    public EnemyController Controller { get { return _controller; } }

    public EnemyState(EnemyController controller)
    {
        _controller = controller;
    }
}
