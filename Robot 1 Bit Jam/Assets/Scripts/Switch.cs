using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
    [SerializeField] private List<SwitchPlatform> platformsA;
    [SerializeField] private List<SwitchPlatform> platformsB;

    private bool _switchState = true;

    private void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        foreach (SwitchPlatform platform in platformsB)
        {
            platform.gameObject.SetActive(false);
        }
    }

    private void SwitchState()
    {
        _switchState = !_switchState;

        if (_switchState)
        {
            foreach (SwitchPlatform platform in platformsA)
            {
                platform.gameObject.SetActive(true);
            }

            foreach (SwitchPlatform platform in platformsB)
            {
                platform.gameObject.SetActive(false);
            }
        } else
        {
            foreach (SwitchPlatform platform in platformsB)
            {
                platform.gameObject.SetActive(true);
            }

            foreach (SwitchPlatform platform in platformsA)
            {
                platform.gameObject.SetActive(false);
            }
        }
    }

    public void Activate()
    {
        SwitchState();
    }
}
