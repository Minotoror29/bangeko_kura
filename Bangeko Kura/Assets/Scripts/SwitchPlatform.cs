using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SwitchPlatformState { Entering, Exiting, Idle }

[RequireComponent(typeof(Animator))]
public class SwitchPlatform : MonoBehaviour
{
    private Animator _animator;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private List<Collider2D> colliders;
    [SerializeField] private List<Collider2D> voidColliders;

    private bool _active = true;

    private EventInstance _spawnSound;

    public bool Active { get { return _active; } }

    public void Initialize(bool active)
    {
        _animator = GetComponent<Animator>();

        spriteRenderer.enabled = active;

        foreach (Collider2D collider in colliders)
        {
            collider.enabled = active;
        }

        foreach (Collider2D collider in voidColliders)
        {
            collider.enabled = !active;
        }

        _active = active;

        _spawnSound = RuntimeManager.CreateInstance("event:/Environment/Switch Platform Spawn");
    }

    public void Enter()
    {
        gameObject.SetActive(true);
        foreach (Collider2D collider in colliders)
        {
            collider.enabled = true;
        }
        foreach (Collider2D collider in voidColliders)
        {
            collider.enabled = false;
        }
        _animator.CrossFade("Enter", 0f);
        _spawnSound.start();

        _active = true;
    }

    public void Exit()
    {
        foreach (Collider2D collider in colliders)
        {
            collider.enabled = false;
        }
        foreach (Collider2D collider in voidColliders)
        {
            collider.enabled = true;
        }
        _animator.CrossFade("Exit", 0f);

        _active = false;
    }
}
