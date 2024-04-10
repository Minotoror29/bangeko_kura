using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InGameCutsceneManager : MonoBehaviour
{
    [SerializeField] private List<InGameCutsceneEvent> events;

    private int _currentEvent;

    private float _eventTimer;

    [SerializeField] private UnityEvent OnCutsceneEnd;

    public void StartCutscene()
    {
        events[_currentEvent].cutsceneEvents?.Invoke();
        _eventTimer = events[_currentEvent].eventTime;
    }

    private void NextEvent()
    {
        _currentEvent++;

        if (_currentEvent >= events.Count)
        {
            OnCutsceneEnd?.Invoke();

            return;
        }

        events[_currentEvent].cutsceneEvents?.Invoke();
        _eventTimer = events[_currentEvent].eventTime;
    }

    private void Update()
    {
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
