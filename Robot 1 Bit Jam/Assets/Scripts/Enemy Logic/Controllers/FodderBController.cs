using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FodderBController : EnemyController
{
    public override void EnemyDiedClose(Transform source)
    {
        ChangeState(new EnemyFleeFromState(this, source));
    }
}
