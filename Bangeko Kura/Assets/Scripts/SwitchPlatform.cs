using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SwitchPlatformState { Entering, Exiting, Idle }

public class SwitchPlatform : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Collider2D coll;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private SwitchPlatformState _currentState = SwitchPlatformState.Idle;

    [SerializeField] private float animationTime = 0.5f;
    private float _animationTimer = 0f;

    public void Enter()
    {
        //_currentState = SwitchPlatformState.Entering;
        //_animationTimer = animationTime;

        gameObject.SetActive(true);
        coll.enabled = true;
        animator.CrossFade("Enter", 0f);
    }

    public void Exit()
    {
        //_currentState = SwitchPlatformState.Exiting;
        //_animationTimer = animationTime;

        coll.enabled = false;
        animator.CrossFade("Exit", 0f);
    }

    private void Update()
    {
        //if (_currentState != SwitchPlatformState.Idle)
        //{
        //    if (_animationTimer > 0f)
        //    {
        //        _animationTimer -= Time.deltaTime;
        //    } else
        //    {
        //        _currentState = SwitchPlatformState.Idle;
        //    }
        //}
    }
}
