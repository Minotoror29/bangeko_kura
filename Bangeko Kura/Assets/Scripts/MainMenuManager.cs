using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private List<TimedAction> menuEvents;
    [SerializeField] private ActionSequence onPlaySequence;

    private MenuControls _menuControls;

    private int _currentEvent = 0;
    private float _eventTimer = 0f;

    private void OnEnable()
    {
        onPlaySequence.OnSequenceEnd += LoadFirstLevel;
    }

    private void OnDisable()
    {
        onPlaySequence.OnSequenceEnd -= LoadFirstLevel;
    }

    private void Start()
    {
        _menuControls = new MenuControls();
        _menuControls.Menu.Enable();
        _menuControls.Menu.Skip.performed += ctx => SkipIntro();
        
        PlayEvent(0);
    }

    private void SkipIntro()
    {
        if (_currentEvent < 7)
        {
            _currentEvent = 7;
            PlayEvent(_currentEvent);
        }
    }

    private void PlayEvent(int eventIndex)
    {
        _eventTimer = menuEvents[eventIndex].Time;
        menuEvents[eventIndex].Events?.Invoke();
    }

    private void Update()
    {
        if (_eventTimer > 0f)
        {
            _eventTimer -= Time.deltaTime;

            if ( _eventTimer <= 0f )
            {
                _currentEvent++;
                if (_currentEvent <  menuEvents.Count)
                {
                    PlayEvent(_currentEvent);
                }
            }
        }

        onPlaySequence.UpdateLogic();
    }

    public void Play()
    {
        onPlaySequence.StartSequence();
    }

    private void LoadFirstLevel()
    {
        SceneManager.LoadScene(1);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
