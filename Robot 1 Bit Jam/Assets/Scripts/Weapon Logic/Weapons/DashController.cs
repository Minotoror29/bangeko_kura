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

    public override void Initialize(Controller controller, HealthSystem healthSystem)
    {
        base.Initialize(controller, healthSystem);

        _target = FindObjectOfType<PlayerController>().transform;

        _dashCooldownTimer = 0f;
        _dashTimer = 0f;
        Controller.Dashing = false;
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
                if ((_target.position - Controller.transform.position).magnitude >= distanceToDash)
                {
                    Controller.Dashing = true;
                    _dashDirection = (_target.position - Controller.transform.position).normalized;
                    _dashCooldownTimer = 0f;
                }
            }
        } else
        {
            if (_dashTimer < dashTime)
            {
                _dashTimer += Time.deltaTime;
            } else
            {
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
