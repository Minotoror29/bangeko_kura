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
            Controller.ChangeState(new EnemyChasingState(Controller));
        }
    }
}
