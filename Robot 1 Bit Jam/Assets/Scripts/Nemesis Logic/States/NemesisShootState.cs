using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NemesisShootState : NemesisState
{
    private float _timer;

    public NemesisShootState(NemesisController controller) : base(controller)
    {
        _timer = 0f;
    }

    public override void Enter()
    {
    }

    public override void Exit()
    {
    }

    public override void OnCollisionEnter(Collision2D collision)
    {
    }

    public override void UpdateLogic()
    {
        if (_timer < 2f)
        {
            _timer += Time.deltaTime;
        } else
        {
            if ((Player.transform.position - Controller.transform.position).magnitude > 1.5f)
            {
                Controller.ChangeState(new NemesisFarState(Controller));
            } else
            {
                Controller.ChangeState(new NemesisSwordChargeState(Controller));
            }
        }
    }

    public override void UpdatePhysics()
    {
        Controller.StopMovement();
    }
}
