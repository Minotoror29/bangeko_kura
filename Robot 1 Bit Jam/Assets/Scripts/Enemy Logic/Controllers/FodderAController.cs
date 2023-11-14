using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FodderAController : EnemyController
{
    public override void EnemyDiedClose(Transform source)
    {
        base.EnemyDiedClose(source);

        ChangeState(new EnemyFleeFromState(this, source));
    }
}
