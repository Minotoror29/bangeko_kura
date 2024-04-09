using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private static CameraManager _instance;
    public static CameraManager Instance => _instance;

    private CinemachineVirtualCamera _currentCamera;
    private CinemachineBasicMultiChannelPerlin _perlin;
    private float _shakeTimer = 0f;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        } else
        {
            Destroy(gameObject);
        }
    }

    public void ChangeCamera(CinemachineVirtualCamera nextCamera)
    {
        if (_currentCamera != null)
        {
            _currentCamera.gameObject.SetActive(false);
        }
        _currentCamera = nextCamera;
        _currentCamera.gameObject.SetActive(true);
        _perlin = _currentCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void ShakeCamera(float gain, float time)
    {
        _perlin.m_AmplitudeGain = gain;
        _shakeTimer = time;
    }

    private void Update()
    {
        if (_shakeTimer > 0f)
        {
            _shakeTimer -= Time.deltaTime;
        } else
        {
            _perlin.m_AmplitudeGain = 0f;
        }
    }
}
