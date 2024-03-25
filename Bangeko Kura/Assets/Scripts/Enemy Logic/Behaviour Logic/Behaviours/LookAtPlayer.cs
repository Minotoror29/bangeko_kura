using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPlayer : EnemyBehaviour
{
    public LookAtPlayer(EnemyController controller) : base(controller)
    {
    }

    public override void SubscribeEvents(EnemyState state)
    {
        state.OnUpdate += Look;
    }

    public override void UnsubscribeEvents(EnemyState state)
    {
        state.OnUpdate -= Look;
    }

    private void Look()
    {
        Controller.LookTowards(Controller.Player.transform.position - Controller.transform.position, true);
    }
}
