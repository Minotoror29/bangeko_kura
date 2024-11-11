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
    [Header("TESTING ONLY")]
    [SerializeField] private bool testing = false;

    [Space, Space, Space, Space]
    [SerializeField] private Image background;
    [SerializeField] private float backgroundOpacity = 1f;
    [SerializeField] private List<CutsceneFrame> frames;
    private float _frameTimer;
    private int _currentFrameindex;

    [SerializeField] private int soundIndex;
    [SerializeField] private string soundPath;
    private EventInstance _sound;

    [SerializeField] private bool stayLastFrame = false;

    private bool _cutsceneEnded = false;
    private bool _fadingBackground = false;
    private float _fadingBackgroundTimer = 1f;

    [SerializeField] private UnityEvent OnCutsceneStart;
    [SerializeField] private UnityEvent OnCutsceneEnd;

    private void Start()
    {
        if (testing)
        {
            Initialize();
            StartCutscene();
        }
    }

    private void Update()
    {
        if (testing)
        {
            UpdateLogic();
        }
    }

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

        if (backgroundOpacity > 1f)
        {
            background.gameObject.SetActive(true);
        } else
        {
            background.color = new Color(0f, 0f, 0f, 0f);
            background.gameObject.SetActive(true);
            _fadingBackground = true;
        }
        
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

                frames[0].gameObject.SetActive(true);

                if (soundIndex == 0)
                {
                    _sound.start();
                }

                _frameTimer = frames[0].FrameTime;
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
        if (frames[_currentFrameindex].Shake)
        {
            frames[_currentFrameindex].StartShake();
        }
        if (soundIndex == _currentFrameindex && soundPath != "")
        {
            _sound.start();
        }
    }

    public void UpdateLogic()
    {
        if (_cutsceneEnded) return;

        if (_fadingBackground)
        {
            if (_fadingBackgroundTimer > 0f)
            {
                _fadingBackgroundTimer -= Time.deltaTime;
                background.color = new Color(0f, 0f, 0f, Mathf.Abs((_fadingBackgroundTimer - 1f) * backgroundOpacity));

                if (_fadingBackgroundTimer <= 0f)
                {
                    _fadingBackground = false;

                    frames[0].gameObject.SetActive(true);

                    if (soundIndex == 0)
                    {
                        _sound.start();
                    }

                    _frameTimer = frames[0].FrameTime;
                }
            }
        } else
        {
            if (_frameTimer > 0f)
            {
                _frameTimer -= Time.deltaTime;
            }
            else
            {
                NextFrame();
            }
        }
    }
}
