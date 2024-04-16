using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SwitchPlatformState { Entering, Exiting, Idle }

public class SwitchPlatform : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Collider2D coll;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private EventInstance _spawnSound;

    public void Initialize()
    {
        _spawnSound = RuntimeManager.CreateInstance("event:/Environment/Switch Platform Spawn");
    }

    public void Enter()
    {
        gameObject.SetActive(true);
        coll.enabled = true;
        animator.CrossFade("Enter", 0f);
        _spawnSound.start();
    }

    public void Exit()
    {
        coll.enabled = false;
        animator.CrossFade("Exit", 0f);
    }
}
