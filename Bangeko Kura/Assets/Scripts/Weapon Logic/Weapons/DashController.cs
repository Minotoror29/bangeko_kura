using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashController : Weapon
{
    [SerializeField] private float dashCooldown = 6f;
    private float _dashCooldownTimer;

    [SerializeField] private float dashTime = 0.1f;
    [SerializeField] private float dashSpeed = 1000f;
    private float _dashTimer;

    [SerializeField] private float distanceToDash = 15f;
    private Transform _target;
    private Vector3 _dashDirection;

    private EventInstance _dashSound;

    public override void Initialize(Controller controller, HealthSystem healthSystem)
    {
        base.Initialize(controller, healthSystem);

        _target = FindObjectOfType<NewPlayerController>().transform;

        _dashCooldownTimer = 0f;
        _dashTimer = 0f;
        Controller.Dashing = false;

        _dashSound = RuntimeManager.CreateInstance("event:/Movement/Dash");
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        if (!Controller.Dashing)
        {
            if (_dashCooldownTimer < dashCooldown)
            {
                _dashCooldownTimer += Time.deltaTime;
            }
            else
            {
                if ((_target.position - Controller.transform.position).magnitude >= distanceToDash && Controller.Rb.velocity.magnitude > 0)
                {
                    if (Controller.MeshAnimator != null)
                    {
                        Controller.MeshAnimator.SetBool("Dashing", true);
                    }
                    Controller.Dashing = true;
                    _dashDirection = (_target.position - Controller.transform.position).normalized;
                    _dashCooldownTimer = 0f;

                    _dashSound.start();
                }
            }
        } else
        {
            if (_dashTimer < dashTime)
            {
                _dashTimer += Time.deltaTime;
            } else
            {
                if (Controller.MeshAnimator != null)
                {
                    Controller.MeshAnimator.SetBool("Dashing", false);
                }
                Controller.Dashing = false;
                _dashTimer = 0f;
            }
        }
    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();

        if (Controller.Dashing)
        {
            Controller.Rb.velocity = dashSpeed * Time.fixedDeltaTime * _dashDirection.normalized;
        }
    }
}
