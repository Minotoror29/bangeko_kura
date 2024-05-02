using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveToElevatorState : PlayerState
{
    private Elevator _elevator;
    private Vector2 _targetPosition;

    public PlayerMoveToElevatorState(PlayerController controller, Elevator elevator) : base(controller)
    {
        _elevator = elevator;
    }

    public override void Enter()
    {
        Controls.InGame.Disable();
        Controller.SetCollidersActive(false);
        Rb.velocity = Vector2.zero;

        _targetPosition = (Vector2)_elevator.transform.position + Vector2.up * 0.5f;

        Animator.CrossFade("Player Walk Forward", 0f);
    }

    public override void Exit()
    {
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
        Controller.RotateMeshSmooth((_targetPosition - (Vector2)Controller.transform.position).normalized, 1000f);
    }

    public override void UpdatePhysics()
    {
        Vector2 newLocation = Controller.transform.position;
        newLocation = Vector2.MoveTowards(newLocation, _targetPosition, 5f * Time.fixedDeltaTime);
        Controller.Rb.MovePosition(newLocation);

        if (newLocation == _targetPosition)
        {
            _elevator.ChangeState(ElevatorState.Moving);
            Controller.ChangeState(new PlayerWaitElevatorState(Controller, _elevator));
        }
    }
}
