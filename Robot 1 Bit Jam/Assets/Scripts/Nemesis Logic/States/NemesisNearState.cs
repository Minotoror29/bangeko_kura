using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NemesisNearState : NemesisState
{
    public NemesisNearState(NemesisController controller, PlayerController player) : base(controller, player)
    {
    }

    public override void Enter()
    {
        Debug.Log("Near");
    }

    public override void Exit()
    {
    }

    public override void OnCollisionEnter(Collision collision)
    {
    }

    public override void UpdateLogic()
    {
        if ((Player.transform.position - Controller.transform.position).magnitude > 1.5f)
        {
            Controller.ChangeState(new NemesisFarState(Controller, Player));
        }
    }

    public override void UpdatePhysics()
    {
        Controller.Rb.velocity = Vector3.zero;
    }
}
