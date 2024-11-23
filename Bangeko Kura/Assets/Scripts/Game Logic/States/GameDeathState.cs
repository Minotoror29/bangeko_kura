using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum DeathState { Pause, Animation, Negative, Black }

public class GameDeathState : GameState
{
    private DeathState _currentDeathState;

    private bool _fromFall;
    private float _pauseTimer = 1f;
    private float _animationTimer = 2.333f;
    private float _negativeTimer = 1f;
    private float _blackTimer = 1f;

    public GameDeathState(GameManager gameManager, bool fromFall) : base(gameManager)
    {
        _fromFall = fromFall;
    }

    public override void Enter()
    {
        _currentDeathState = DeathState.Pause;
    }

    public override void Exit()
    {
    }

    public override void OnCollisionEnter(Collision2D collision)
    {
    }

    public override void OnCollisionStay(Collision2D collision)
    {
    }

    public override void UpdateLogic()
    {
        GameManager.Player.UpdateLogic();

        if (_currentDeathState == DeathState.Pause)
        {
            _pauseTimer -= Time.deltaTime;

            if (_pauseTimer <= 0f)
            {
                _currentDeathState = DeathState.Animation;
                if (!_fromFall)
                {
                    GameManager.Player.MeshAnimator.speed = 1;
                    GameManager.Player.MeshAnimator.CrossFade("Player Death", 0f);
                    CameraManager.Instance.ChangeCamera(GameManager.DeathAnimationCam);
                }
            }
        } else if (_currentDeathState == DeathState.Animation)
        {
            _animationTimer -= Time.deltaTime;

            if (_animationTimer <= 0f)
            {
                _currentDeathState = DeathState.Negative;
                CameraManager.Instance.ChangeCamera(GameManager.DeathNegativeCam);
                GameManager.InvertVolume.gameObject.SetActive(true);
            }
        } else if (_currentDeathState == DeathState.Negative)
        {
            _negativeTimer -= Time.deltaTime;

            if (_negativeTimer <= 0f)
            {
                _currentDeathState = DeathState.Black;
                GameManager.InvertVolume.gameObject.SetActive(false);
                GameManager.BlackScreen.gameObject.SetActive(true);
            }
        } else if (_currentDeathState == DeathState.Black)
        {
            _blackTimer -= Time.deltaTime;

            if (_blackTimer <= 0f)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    public override void UpdatePhysics()
    {
    }
}
