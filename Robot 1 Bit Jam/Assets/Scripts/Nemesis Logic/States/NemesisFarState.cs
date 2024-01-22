using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NemesisFarState : NemesisState
{
    public NemesisFarState(NemesisController controller) : base(controller)
    {
    }

    public override void Enter()
    {
        int randomBehaviour = Random.Range(0, 2);
        if (randomBehaviour == 0)
        {
            //Controller.ChangeState(new NemesisWalkState(Controller));
        } else if (randomBehaviour == 1)
        {
            Controller.ChangeState(new NemesisShootState(Controller));
        }
    }

    public override void Exit()
    {
    }

    public override void OnCollisionEnter(Collision2D collision)
    {
    }

    public override void UpdateLogic()
    {
    }

    public override void UpdatePhysics()
    {
        Controller.StopMovement();
    }
}
