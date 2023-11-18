using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IntroManager : MonoBehaviour
{
    [SerializeField] private List<TextMeshProUGUI> texts;
    [SerializeField] private Button playButton;
    [SerializeField] private TextMeshProUGUI playButtonText;
    private int _fadingText;

    [SerializeField] private float nextTextTime;
    [SerializeField] private float fadeTime;
    private float _nextTextTimer;
    private float _fadeTimer;

    private Color _textColor;

    private EventInstance _startSound;

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
        _fadingText = 0;
        _nextTextTimer = 0f;
        _fadeTimer = 0f;

        _textColor = new Color(1f, 1f, 1f, 0f);
        foreach (TextMeshProUGUI text in texts)
        {
            text.color = _textColor;
        }

        playButtonText.color = _textColor;
        playButton.gameObject.SetActive(false);

        _startSound = RuntimeManager.CreateInstance("event:/Game Start");
    }

    public void UpdateLogic()
    {
        if (_fadingText < texts.Count)
        {
            if (_nextTextTimer < nextTextTime)
            {
                _nextTextTimer += Time.deltaTime;

                if (_fadeTimer < fadeTime)
                {
                    _fadeTimer += Time.deltaTime;
                    _textColor.a = _fadeTimer / fadeTime;
                    texts[_fadingText].color = _textColor;
                }
            }
            else
            {
                _fadingText++;
                _fadeTimer = 0f;
                _nextTextTimer = 0f;
                _textColor.a = 0f;

                if (_fadingText == texts.Count)
                {
                    playButton.gameObject.SetActive(true);
                }
            }
        } else
        {
            if (_fadeTimer < fadeTime)
            {
                _fadeTimer += Time.deltaTime;
                _textColor.a = _fadeTimer / fadeTime;
                playButtonText.color = _textColor;
            }
        }
    }

    public void Play()
    {
        _startSound.start();

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
