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
            Vector3 startPosition = Controller.transform.position + Controller.GetComponent<CircleCollider2D>().radius * Controller.transform.localScale.x * (Controller.Player.transform.position - Controller.transform.position).normalized;
            Ray ray = new(startPosition, Controller.Player.transform.position - startPosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, (Controller.Player.transform.position - Controller.transform.position).magnitude, Controller.VoidLayer);
            if (hit.collider == null)
            {
                Controller.ChangeState(new EnemyChasingState(Controller));
            }
        }
    }
}
