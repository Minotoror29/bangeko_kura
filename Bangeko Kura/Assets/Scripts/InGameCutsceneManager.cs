using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InGameCutsceneManager : MonoBehaviour
{
    [SerializeField] private List<InGameCutsceneEvent> events;

    private int _currentEvent;

    private float _eventTimer;

    private bool _cutsceneEnded = false;

    [SerializeField] private UnityEvent OnCutsceneStart;
    [SerializeField] private UnityEvent OnCutsceneEnd;

    public void StartCutscene()
    {
        events[_currentEvent].cutsceneEvents?.Invoke();
        _eventTimer = events[_currentEvent].eventTime;

        OnCutsceneStart?.Invoke();
    }

    private void NextEvent()
    {
        _currentEvent++;

        if (_currentEvent >= events.Count)
        {
            OnCutsceneEnd?.Invoke();
            _cutsceneEnded = true;

            return;
        }

        events[_currentEvent].cutsceneEvents?.Invoke();
        _eventTimer = events[_currentEvent].eventTime;
    }

    private void Update()
    {
        if (_cutsceneEnded) return;

        if (_eventTimer > 0f)
        {
            _eventTimer -= Time.deltaTime;

            if (_eventTimer <= 0f)
            {
                NextEvent();
            }
        }
    }
}
