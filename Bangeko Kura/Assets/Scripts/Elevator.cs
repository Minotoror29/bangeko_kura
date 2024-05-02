using FMOD.Studio;
using FMODUnity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum ElevatorState { Idle, Moving, Waiting }

public class Elevator : MonoBehaviour
{
    private ElevatorState _currentState;

    [SerializeField] private Transform startPosition;
    [SerializeField] private Transform endPosition;
    private Vector2 _currentTarget;

    [SerializeField] private float elevatorSpeed;

    [SerializeField] private bool oneWayElevator;

    private EventInstance _arrivalSound;
    private EventInstance _movingSound;

    public UnityEvent OnArrival;

    public ElevatorState CurrentState { get { return _currentState; } }

    private void Awake()
    {
        _arrivalSound = RuntimeManager.CreateInstance("event:/Environment/Elevator Arrival");
        _movingSound = RuntimeManager.CreateInstance("event:/Environment/Elevator Moving Loop");
    }

    private void Start()
    {
        _currentTarget = endPosition.position;
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
        } else if (_currentState == ElevatorState.Moving)
        {
            _movingSound.start();
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
                _movingSound.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                _arrivalSound.start();
                OnArrival?.Invoke();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (oneWayElevator) return;

        if (collision.TryGetComponent(out PlayerController player))
        {
            ChangeState(ElevatorState.Idle);
        }
    }
}
