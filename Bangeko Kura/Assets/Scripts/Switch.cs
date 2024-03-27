using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SwitchState { AState, BState, SwitchingToA, SwitchingToB }

public class Switch : MonoBehaviour
{
    [SerializeField] private List<SwitchPlatform> platformsA;
    [SerializeField] private List<SwitchPlatform> platformsB;

    private SwitchState _currentState;

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

        _currentState = SwitchState.AState;
    }

    public void Activate()
    {
        ChangeState();
    }

    private void ChangeState()
    {
        if (_currentState == SwitchState.AState)
        {
            _currentState = SwitchState.SwitchingToB;
            StartCoroutine(EnterPlatforms(SwitchState.BState, platformsB, platformsA));
        } else if (_currentState == SwitchState.BState)
        {
            _currentState = SwitchState.SwitchingToA;
            StartCoroutine(EnterPlatforms(SwitchState.AState, platformsA, platformsB));
        }
    }

    private IEnumerator EnterPlatforms(SwitchState stateToSwitchTo, List<SwitchPlatform> platformsToEnter, List<SwitchPlatform> platformsToExit)
    {
        foreach (SwitchPlatform platform in platformsToEnter)
        {
            platform.Enter();
        }

        yield return new WaitForSeconds(0.5f);

        StartCoroutine(ExitPlatforms(stateToSwitchTo, platformsToExit));
    }

    private IEnumerator ExitPlatforms(SwitchState stateToSwitchTo, List<SwitchPlatform> platforms)
    {
        foreach (SwitchPlatform platform in platforms)
        {
            platform.Exit();
        }

        yield return new WaitForSeconds(0.5f);

        _currentState = stateToSwitchTo;
    }
}
