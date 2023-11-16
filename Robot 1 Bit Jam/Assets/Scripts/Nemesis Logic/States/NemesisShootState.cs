using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NemesisShootState : NemesisState
{
    private float _timer;

    public NemesisShootState(NemesisController controller, PlayerController player) : base(controller, player)
    {
        _timer = 0f;
    }

    public override void Enter()
    {
        Debug.Log("Shoot");
    }

    public override void Exit()
    {
    }

    public override void OnCollisionEnter(Collision collision)
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
                Controller.ChangeState(new NemesisFarState(Controller, Player));
            } else
            {
                Controller.ChangeState(new NemesisNearState(Controller, Player));
            }
        }
    }

    public override void UpdatePhysics()
    {
        Controller.Rb.velocity = Vector3.zero;
    }
}
