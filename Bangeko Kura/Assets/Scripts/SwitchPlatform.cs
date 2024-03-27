using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SwitchPlatformState { Entering, Exiting, Idle }

public class SwitchPlatform : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Collider2D coll;
    [SerializeField] private SpriteRenderer spriteRenderer;

    public void Enter()
    {
        gameObject.SetActive(true);
        coll.enabled = true;
        animator.CrossFade("Enter", 0f);
    }

    public void Exit()
    {
        coll.enabled = false;
        animator.CrossFade("Exit", 0f);
    }
}
