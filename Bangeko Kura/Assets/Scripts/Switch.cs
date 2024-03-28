using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SwitchState { AState, BState, SwitchingToA, SwitchingToB }

public class Switch : MonoBehaviour
{
    [SerializeField] private Animator animator;

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
        animator.CrossFade("Totem Shake", 0f);

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

        yield return new WaitForSeconds(0.604f);

        StartCoroutine(ExitPlatforms(stateToSwitchTo, platformsToExit));
    }

    private IEnumerator ExitPlatforms(SwitchState stateToSwitchTo, List<SwitchPlatform> platforms)
    {
        foreach (SwitchPlatform platform in platforms)
        {
            platform.Exit();
        }

        yield return new WaitForSeconds(0.604f);

        _currentState = stateToSwitchTo;

        animator.CrossFade("Totem Idle", 0f);
    }
}
