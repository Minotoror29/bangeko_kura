using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NemesisShootState : NemesisState
{
    private float _shootTimer;
    private int _projectilesToShoot;

    public NemesisShootState(NemesisController controller) : base(controller)
    {
    }

    public override void Enter()
    {
        _shootTimer = Controller.ShootTime;
        _projectilesToShoot = 3;
    }

    public override void Exit()
    {
    }

    public override void OnCollisionEnter(Collision2D collision)
    {
    }

    public override void UpdateLogic()
    {
        if (_shootTimer > 0f)
        {
            _shootTimer -= Time.deltaTime;
        } else
        {
            Controller.ShootBullet();
            _projectilesToShoot--;

            if (_projectilesToShoot == 0)
            {
                Controller.ChangeState(new NemesisWalkState(Controller, 2f));
            } else
            {
                _shootTimer = Controller.ShootTime;
            }
        }
    }

    public override void UpdatePhysics()
    {
        Rb.velocity = Vector2.zero;
    }
}
