using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NemesisMoveState : NemesisState
{
    private float _timer;

    public NemesisMoveState(NemesisController controller, NewPlayerController player) : base(controller, player)
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
        if (_timer < 5f)
        {
            _timer += Time.deltaTime;
        } else
        {
            Controller.ChangeState(new NemesisShootState(Controller, Player));
        }

        if ((Player.transform.position - Controller.transform.position).magnitude <= 1.5f)
        {
            Controller.ChangeState(new NemesisNearState(Controller, Player));
        }
    }

    public override void UpdatePhysics()
    {
        Controller.MoveTowards(Player.transform.position - Controller.transform.position);
    }
}
