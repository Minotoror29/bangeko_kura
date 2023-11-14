using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FodderBController : EnemyController
{
    [SerializeField] private float chasingDistance = 15f;

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        if (DistanceToPlayer <= chasingDistance)
        {
            ChangeState(new EnemyChasingState(this));
        }
    }
}
