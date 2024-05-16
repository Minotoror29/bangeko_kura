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

    private EventInstance _spawnSound;

    public void Initialize(bool active)
    {
        _animator = GetComponent<Animator>();
        //_colliders = new();
        //foreach (Collider2D collider in GetComponentsInChildren<Collider2D>())
        //{
        //    _colliders.Add(collider);
        //}

        spriteRenderer.enabled = active;

        foreach (Collider2D collider in colliders)
        {
            collider.enabled = active;
        }

        foreach (Collider2D collider in voidColliders)
        {
            collider.enabled = !active;
        }

        _spawnSound = RuntimeManager.CreateInstance("event:/Environment/Switch Platform Spawn");
    }

    public void Enter()
    {
        gameObject.SetActive(true);
        foreach (Collider2D collider in colliders)
        {
            collider.enabled = true;
            //collider.tag = "Ground";
            //collider.gameObject.layer = LayerMask.NameToLayer("Ground");
        }
        foreach (Collider2D collider in voidColliders)
        {
            collider.enabled = false;
        }
        _animator.CrossFade("Enter", 0f);
        _spawnSound.start();
    }

    public void Exit()
    {
        foreach (Collider2D collider in colliders)
        {
            collider.enabled = false;
            //collider.tag = "Obstacle";
            //collider.gameObject.layer = LayerMask.NameToLayer("Void");
        }
        foreach (Collider2D collider in voidColliders)
        {
            collider.enabled = true;
        }
        _animator.CrossFade("Exit", 0f);
    }
}
