using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrolState : EnemyState
{
    private Vector3 _target;

    public EnemyPatrolState(EnemyController controller) : base(controller)
    {
    }

    public override void Enter()
    {
        float randomAngle = Random.Range(0f, 360f);
        float x = Controller.transform.position.x + Controller.PatrolRadius * Mathf.Cos(randomAngle);
        float z = Controller.transform.position.z + Controller.PatrolRadius * Mathf.Sin(randomAngle);
        _target = new Vector3(x, 0f, z);
    }

    public override void Exit()
    {
    }

    public override void UpdateLogic()
    {
        if ((_target - Controller.transform.position).magnitude < 0.01f)
        {
            Controller.ChangeState(new EnemyIdleState(Controller, Controller.GetRandomIdleTime()));
        }
    }

    public override void UpdatePhysics()
    {
        Controller.MoveTo(_target);
    }
}
