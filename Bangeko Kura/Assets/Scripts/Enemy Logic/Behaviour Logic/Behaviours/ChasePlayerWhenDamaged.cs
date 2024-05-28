using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasePlayerWhenDamaged : EnemyBehaviour
{
    public ChasePlayerWhenDamaged(EnemyController controller) : base(controller)
    {
    }

    public override void SubscribeEvents(EnemyState state)
    {
        state.OnUpdate += DamagedByPlayer;
    }

    public override void UnsubscribeEvents(EnemyState state)
    {
        state.OnUpdate -= DamagedByPlayer;
    }

    private void DamagedByPlayer()
    {
        if (Controller.DamagedByPlayer)
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
