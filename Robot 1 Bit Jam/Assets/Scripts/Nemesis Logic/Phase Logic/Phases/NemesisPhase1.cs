using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NemesisPhase1 : NemesisPhase
{
    public NemesisPhase1(NemesisPhaseData data, NemesisController controller) : base(data, controller)
    {
    }

    public override void Enter()
    {
        ChangeState(new NemesisIdleState(this, CheckPlayerDistance));
    }

    private void GoToIdleState()
    {
        ChangeState(new NemesisIdleState(this, CheckPlayerDistance));
    }

    private void GoToWalkState()
    {
        ChangeState(new NemesisWalkState(this, 3f, GoToIdleState));
    }

    private void CheckPlayerDistance()
    {
        if ((Player.transform.position - Controller.transform.position).magnitude <= Controller.SwordDistance && Controller.SwordCooldownTimer <= 0f)
        {
            ChangeState(new NemesisSwordChargeState(this, GoToIdleState));
        }
        else if ((Player.transform.position - Controller.transform.position).magnitude <= Controller.WalkDistance)
        {
            ChangeState(new NemesisWalkState(this, 3f, GoToIdleState));
        }
        else if ((Player.transform.position - Controller.transform.position).magnitude <= Controller.ShootDistance)
        {
            ChangeState(new NemesisShootState(this, GoToWalkState));
        }
        else if ((Player.transform.position - Controller.transform.position).magnitude > Controller.ShootDistance)
        {
            ChangeState(new NemesisDashState(this, 1, CheckPlayerDistanceAfterDash, GoToIdleState));
        }
    }

    private void CheckPlayerDistanceAfterDash(int dashNumber)
    {
        if ((Player.transform.position - Controller.transform.position).magnitude <= Controller.WalkDistance)
        {
            ChangeState(new NemesisSwordAttackState(this, GoToIdleState));
        }
        else if ((Player.transform.position - Controller.transform.position).magnitude <= Controller.ShootDistance)
        {
            ChangeState(new NemesisShootState(this, GoToWalkState));
        }
        else
        {
            if (dashNumber == 1)
            {
                ChangeState(new NemesisDashState(this, 2, CheckPlayerDistanceAfterDash, GoToIdleState));
            }
            else if (dashNumber == 2)
            {
                ChangeState(new NemesisShootState(this, GoToWalkState));
            }
        }
    }
}
