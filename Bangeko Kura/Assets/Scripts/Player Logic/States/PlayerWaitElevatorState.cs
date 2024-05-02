using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWaitElevatorState : PlayerState
{
    private Elevator _elevator;

    public PlayerWaitElevatorState(PlayerController controller, Elevator elevator) : base(controller)
    {
        _elevator = elevator;
    }

    public override void Enter()
    {
        Controls.InGame.Disable();
        Controller.SetCollidersActive(false);
        Rb.velocity = Vector2.zero;

        Animator.CrossFade("Player Idle", 0f);

        _elevator.OnArrival.AddListener(GoToIdle);
    }

    public override void Exit()
    {
        Controller.AddGround(_elevator.gameObject);
    }

    private void GoToIdle()
    {
        Controller.ChangeState(new PlayerIdleState(Controller));
    }

    public override void OnCollisionEnter(Collision2D collision)
    {
    }

    public override void OnCollisionStay(Collision2D collision)
    {
    }

    public override void OnTriggerEnter(Collider2D collision)
    {
    }

    public override void UpdateLogic()
    {
        Controller.RotateMeshSmooth(Vector3.down, 500f);

        Controller.transform.position = (Vector2)_elevator.transform.position + Vector2.up * 0.5f;
    }

    public override void UpdatePhysics()
    {
    }
}
