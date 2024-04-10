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
    [SerializeField] private List<Image> frames;
    [SerializeField] private float frameTime = 1f;
    private float _frameTimer;
    private int _currentFrameindex;

    [SerializeField] private int soundIndex;
    [SerializeField] private string soundPath;
    private EventInstance _sound;

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

        frames[0].gameObject.SetActive(true);

        if (soundIndex == 0)
        {
            _sound.start();
        }

        _frameTimer = frameTime;
    }

    private void NextFrame()
    {
        frames[_currentFrameindex].gameObject.SetActive(false);
        _currentFrameindex++;

        if (_currentFrameindex == frames.Count)
        {
            OnCutsceneEnd?.Invoke();

            return;
        }

        frames[_currentFrameindex].gameObject.SetActive(true);
        if (soundIndex == _currentFrameindex && soundPath != "")
        {
            _sound.start();
        }
    }

    public void UpdateLogic()
    {
        if (_currentFrameindex == frames.Count) return;

        if (_frameTimer > 0f)
        {
            _frameTimer -= Time.deltaTime;
        } else
        {
            _frameTimer = frameTime;
            NextFrame();
        }
    }
}
