using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : EnemyBehaviour
{
    private float _idleTime;
    private float _idleTimer;

    public Patrol(EnemyController controller) : base(controller)
    {
        _idleTime = controller.GetRandomIdleTime();
        _idleTimer = 0f;
    }

    public override void SubscribeEvents(EnemyState state)
    {
        state.OnUpdate += IdleTimer;
    }

    public override void UnsubscribeEvents(EnemyState state)
    {
        state.OnUpdate -= IdleTimer;
    }

    private void IdleTimer()
    {
        if (_idleTimer < _idleTime)
        {
            _idleTimer += Time.deltaTime;
        } else
        {
            Controller.ChangeState(new EnemyPatrolState(Controller));
        }
    }
}
