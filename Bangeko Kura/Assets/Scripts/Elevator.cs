using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ElevatorState { Idle, Moving, Waiting }

public class Elevator : MonoBehaviour
{
    private ElevatorState _currentState;

    [SerializeField] private Transform startPosition;
    [SerializeField] private Transform endPosition;
    private Vector2 _currentTarget;

    [SerializeField] private float elevatorSpeed;

    public event Action OnArrival;

    public ElevatorState CurrentState { get { return _currentState; } }

    private void Start()
    {
        ChangeState(ElevatorState.Idle);
    }

    public void ChangeState(ElevatorState nextState)
    {
        _currentState = nextState;

        if (_currentState == ElevatorState.Idle)
        {
            if (transform.position == startPosition.position)
            {
                _currentTarget = endPosition.position;
            } else if (transform.position == endPosition.position)
            {
                _currentTarget = startPosition.position;
            }
        }
    }

    private void Update()
    {
        if (_currentState == ElevatorState.Moving)
        {
            Vector3 nextPosition = Vector3.MoveTowards(transform.position, _currentTarget, elevatorSpeed * Time.deltaTime);
            transform.position = nextPosition;

            if ((Vector2)transform.position == _currentTarget)
            {
                ChangeState(ElevatorState.Waiting);
                OnArrival?.Invoke();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out NewPlayerController player))
        {
            ChangeState(ElevatorState.Idle);
        }
    }
}
