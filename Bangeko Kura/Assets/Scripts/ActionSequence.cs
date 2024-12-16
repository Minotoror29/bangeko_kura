using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ActionSequence
{
    [SerializeField] private List<TimedAction> actions;

    private int _currentAction = 0;
    private float _currentActionTimer = 0f;
    private bool _isOver = false;

    public event Action OnSequenceEnd;

    public bool IsOver {  get { return _isOver; } }

    public void StartSequence()
    {
        PlayAction();
    }

    public void UpdateLogic()
    {
        if (_isOver) return;

        if (_currentActionTimer > 0f)
        {
            _currentActionTimer -= Time.deltaTime;

            if (_currentActionTimer <= 0f)
            {
                NextAction();
            }
        }
    }

    private void NextAction()
    {
        _currentAction++;

        if (_currentAction == actions.Count)
        {
            _isOver = true;
            OnSequenceEnd?.Invoke();
            return;
        }

        PlayAction();
    }

    private void PlayAction()
    {
        _currentActionTimer = actions[_currentAction].Time;
        actions[_currentAction].Events?.Invoke();
    }
}
