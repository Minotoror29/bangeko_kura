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
    private List<Collider2D> _colliders;


    private EventInstance _spawnSound;

    public void Initialize()
    {
        _animator = GetComponent<Animator>();
        _colliders = new();
        foreach (Collider2D collider in GetComponentsInChildren<Collider2D>())
        {
            _colliders.Add(collider);
        }

        _spawnSound = RuntimeManager.CreateInstance("event:/Environment/Switch Platform Spawn");
    }

    public void Enter()
    {
        gameObject.SetActive(true);
        foreach (Collider2D collider in _colliders)
        {
            collider.enabled = true;
        }
        _animator.CrossFade("Enter", 0f);
        _spawnSound.start();
    }

    public void Exit()
    {
        foreach (Collider2D collider in _colliders)
        {
            collider.enabled = false;
        }
        _animator.CrossFade("Exit", 0f);
    }
}
