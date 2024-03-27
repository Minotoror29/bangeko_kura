using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyState : State
{
    private EnemyController _controller;

    private EnemyStateId _id;

    private List<EnemyBehaviour> _behaviours;

    public EnemyController Controller { get { return _controller; } }
    public EnemyStateId Id { set { _id = value; } }

    public event Action OnUpdate;

    public EnemyState(EnemyController controller)
    {
        _controller = controller;
    }

    public override void Enter()
    {
        _behaviours = new();

        foreach (EnemyBehaviourData behaviour in Controller.Behaviours)
        {
            if (behaviour.states.Contains(_id) || behaviour.states.Contains(EnemyStateId.All))
            {
                EnemyBehaviour newBehaviour = behaviour.Behaviour(_controller);
                newBehaviour.SubscribeEvents(this);
                _behaviours.Add(newBehaviour);
            }
        }
    }

    public override void Exit()
    {
        foreach (EnemyBehaviour behaviour in _behaviours)
        {
             behaviour.UnsubscribeEvents(this);
        }
    }

    public override void UpdateLogic()
    {
        OnUpdate?.Invoke();
    }

    public virtual bool CanAttackSword(float buildupTime)
    {
        return false;
    }
}
