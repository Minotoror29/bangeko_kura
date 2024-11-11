using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CutsceneFrame : MonoBehaviour
{
    [SerializeField] private float frameTime = 2f;
    [SerializeField] private bool shake = false;

    private float _shakeAmount = 10f;
    private float _shakeTime = 0f;
    private Vector2 _startPosition;

    private float _fadeTimer = 0f;

    public float FrameTime { get { return frameTime; } }
    public bool Shake { get { return shake; } }

    public void StartShake()
    {
        _startPosition = GetComponent<Image>().rectTransform.position;
        _shakeTime = 0.2f;

        StartCoroutine(FadeCountDown());
    }

    private void Update()
    {
        if (_shakeTime > 0f)
        {
            _shakeTime -= Time.deltaTime;

            float shakeX = _startPosition.x + Random.Range(-1f, 1f) * _shakeAmount;
            float shakeY = _startPosition.y + Random.Range(-1f, 1f) * _shakeAmount;
            GetComponent<Image>().rectTransform.position = new Vector2(shakeX, shakeY);

            if (_shakeTime <= 0f)
            {
                GetComponent<Image>().rectTransform.position = _startPosition;
            }
        }

        if (_fadeTimer > 0f)
        {
            _fadeTimer -= Time.deltaTime;
            Color color = new Color(1f, 1f, 1f, _fadeTimer);

            GetComponent<Image>().color = color;
        }
    }

    private IEnumerator FadeCountDown()
    {
        yield return new WaitForSeconds(2f);

        _fadeTimer = 1f;
    }
}
