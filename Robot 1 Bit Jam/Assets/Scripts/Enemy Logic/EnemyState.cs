using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyState : State
{
    private EnemyController _controller;

    private EnemyStateId _id;

    public EnemyController Controller { get { return _controller; } }
    public EnemyStateId Id { set { _id = value; } }

    public event Action<EnemyController> OnUpdate;

    public EnemyState(EnemyController controller)
    {
        _controller = controller;
    }

    public override void Enter()
    {
        foreach (EnemyBehaviourData behaviour in Controller.Behaviours)
        {
            if (behaviour.states.Contains(_id))
            {
                behaviour.SubscribeEvents(this);
            }
        }
    }

    public override void Exit()
    {
        foreach (EnemyBehaviourData behaviour in Controller.Behaviours)
        {
            if (behaviour.states.Contains(_id))
            {
                behaviour.UnsubscribeEvents(this);
            }
        }
    }

    public override void UpdateLogic()
    {
        OnUpdate?.Invoke(Controller);
    }
}
