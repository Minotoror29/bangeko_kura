using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CutsceneManager : MonoBehaviour
{
    [SerializeField] private Image background;
    [SerializeField] private List<CutsceneFrame> frames;
    private float _frameTimer;
    private int _currentFrameindex;

    [SerializeField] private int soundIndex;
    [SerializeField] private string soundPath;
    private EventInstance _sound;

    [SerializeField] private bool stayLastFrame = false;

    private bool _cutsceneEnded = false;

    [SerializeField] private UnityEvent OnCutsceneStart;
    [SerializeField] private UnityEvent OnCutsceneEnd;

    public void Initialize()
    {
        _frameTimer = 0f;

        if (soundPath != "")
        {
            _sound = RuntimeManager.CreateInstance(soundPath);
        }
    }

    public void StartCutscene()
    {
        _currentFrameindex = 0;

        background.gameObject.SetActive(true);
        frames[0].gameObject.SetActive(true);

        if (soundIndex == 0)
        {
            _sound.start();
        }

        _frameTimer = frames[0].FrameTime;

        OnCutsceneStart?.Invoke();
    }

    private void NextFrame()
    {
        frames[_currentFrameindex].gameObject.SetActive(false);
        
        if (_currentFrameindex == frames.Count - 1)
        {
            if (stayLastFrame)
            {
                frames[_currentFrameindex].gameObject.SetActive(true);
            } else
            {
                background.gameObject.SetActive(false);
            }

            OnCutsceneEnd?.Invoke();
            _cutsceneEnded = true;

            return;
        }

        _currentFrameindex++;
        frames[_currentFrameindex].gameObject.SetActive(true);
        _frameTimer = frames[_currentFrameindex].FrameTime;
        if (soundIndex == _currentFrameindex && soundPath != "")
        {
            _sound.start();
        }
    }

    public void UpdateLogic()
    {
        if (_cutsceneEnded) return;

        if (_currentFrameindex == frames.Count) return;

        if (_frameTimer > 0f)
        {
            _frameTimer -= Time.deltaTime;
        } else
        {
            NextFrame();
        }
    }
}
