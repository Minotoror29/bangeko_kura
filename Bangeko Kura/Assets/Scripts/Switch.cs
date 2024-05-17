using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
    [SerializeField] private ScreenManager screenManager;
    private SwitchPlatformManager _switchPlatformManager;

    [SerializeField] private Animator animator;
    private BoxCollider2D _coll;

    private EventInstance _hitSound;

    private void OnEnable()
    {
        screenManager.OnEnter += SetActiveCollider;
        screenManager.OnExit += SetActiveCollider;
    }

    private void OnDisable()
    {
        screenManager.OnEnter -= SetActiveCollider;
        screenManager.OnExit -= SetActiveCollider;
    }

    public void Initialize(SwitchPlatformManager switchPlatformManager)
    {
        _switchPlatformManager = switchPlatformManager;
        _coll = GetComponent<BoxCollider2D>();
        _coll.enabled = false;

        _hitSound = RuntimeManager.CreateInstance("event:/Environment/Switch Hit");
    }

    private void SetActiveCollider(bool active)
    {
        _coll.enabled = active;
    }

    public void Activate()
    {
        _hitSound.start();
        animator.CrossFade("Totem Shake", 0f);
        _switchPlatformManager.ChangeState();
    }
}
