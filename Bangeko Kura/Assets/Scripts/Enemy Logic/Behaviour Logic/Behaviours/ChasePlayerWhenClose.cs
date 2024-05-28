using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasePlayerWhenClose : EnemyBehaviour
{
    private float _chasingDistance;

    public ChasePlayerWhenClose(EnemyController controller, float chasingDistance) : base(controller)
    {
        _chasingDistance = chasingDistance;
    }

    public override void SubscribeEvents(EnemyState state)
    {
        state.OnUpdate += EnterChasingDistance;
    }

    public override void UnsubscribeEvents(EnemyState state)
    {
        state.OnUpdate -= EnterChasingDistance;
    }

    private void EnterChasingDistance()
    {
        if (Controller.DistanceToPlayer <= _chasingDistance && Controller.Player.Mesh.gameObject.activeSelf)
        {
            Ray ray = new(Controller.transform.position, Controller.Player.transform.position - Controller.transform.position);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, (Controller.Player.transform.position - Controller.transform.position).magnitude, Controller.VoidLayer);
            if (hit.collider == null)
            {
                Controller.ChangeState(new EnemyChasingState(Controller));
            }
        }
    }
}
