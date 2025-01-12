using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Intro : MonoBehaviour
{
    [SerializeField] private Canvas blackCanvas;

    private float _soundTimer = 11f;
    private float _timer = 16f;

    private EventInstance _introSound;

    private void Start()
    {
        Cursor.visible = false;

        _introSound = RuntimeManager.CreateInstance("event:/Cutscenes/Intro");
        _introSound.start();
    }

    private void Update()
    {
        if (_timer > 0f)
        {
            _timer -= Time.deltaTime;
        } else
        {
            blackCanvas.gameObject.SetActive(true);
            Cursor.visible = true;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        //if (_soundTimer > 0f)
        //{
        //    _soundTimer -= Time.deltaTime;

        //    if (_soundTimer <= 0f)
        //    {
        //        _introSound.start();
        //    }
        //}
    }
}
