using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDeathState : PlayerState
{
    private bool _fromFall;

    private float _deathAnimationTimer;

    public PlayerDeathState(PlayerController controller, bool fromFall) : base(controller)
    {
        _fromFall = fromFall;
    }

    public override void Enter()
    {
        Controller.SetCollidersActive(false);
        Controls.InGame.Disable();
        
        if (!_fromFall)
        {
            Animator.CrossFade("Player Death", 0f);
            _deathAnimationTimer = 2.333f;
        } else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public override void Exit()
    {
    }

    public override bool CanBeKnockbacked()
    {
        return false;
    }

    public override void OnCollisionEnter(Collision2D collision)
    {
    }

    public override void OnCollisionStay(Collision2D collision)
    {
    }

    public override void OnTriggerEnter(Collider2D collision)
    {
    }

    public override void UpdateLogic()
    {
        if (_deathAnimationTimer > 0f)
        {
            _deathAnimationTimer -= Time.deltaTime;

            if (_deathAnimationTimer <= 0f)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    public override void UpdatePhysics()
    {
    }
}
