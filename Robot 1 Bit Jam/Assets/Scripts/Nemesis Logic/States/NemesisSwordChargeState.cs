using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NemesisSwordChargeState : NemesisState
{
    private float _chargeTimer;

    public NemesisSwordChargeState(NemesisController controller) : base(controller)
    {
        _chargeTimer = controller.SwordChargeTime;
    }

    public override void Enter()
    {
        Rb.velocity = Vector2.zero;
    }

    public override void Exit()
    {
    }

    public override void OnCollisionEnter(Collision2D collision)
    {
    }

    public override void UpdateLogic()
    {
        if (_chargeTimer > 0f)
        {
            _chargeTimer -= Time.deltaTime;
        } else
        {
            Controller.ChangeState(new NemesisSwordAttackState(Controller));
        }
    }

    public override void UpdatePhysics()
    {
        Rb.velocity = Vector2.zero;
    }
}
