using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    private void Start()
    {
        Initialize();
    }

    private void Update()
    {
        UpdateLogic();
    }

    public void Initialize()
    {
        _frameTimer = 0f;
        _currentFrameindex = 0;

        if (soundPath != "")
        {
            _sound = RuntimeManager.CreateInstance(soundPath);
        }

        if (soundIndex == 0)
        {
            _sound.start();
        }
    }

    private void NextFrame()
    {
        frames[_currentFrameindex].gameObject.SetActive(false);
        _currentFrameindex++;

        if (soundIndex == _currentFrameindex && soundPath != "")
        {
            _sound.start();
        }

        if (_currentFrameindex == frames.Count)
        {
            if (SceneManager.GetActiveScene().buildIndex + 1 == SceneManager.sceneCountInBuildSettings)
            {
                Application.Quit();
            } else
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
            return;
        }

        frames[_currentFrameindex].gameObject.SetActive(true);
    }

    public void UpdateLogic()
    {
        if (_currentFrameindex == frames.Count) return;

        if (_frameTimer < frameTime)
        {
            _frameTimer += Time.deltaTime;
        } else
        {
            _frameTimer = 0f;
            NextFrame();
        }
    }
}
