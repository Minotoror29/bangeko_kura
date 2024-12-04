using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum DeathState { Pause, Animation, Negative, Black }

public class GameDeathState : GameState
{
    private DeathState _currentDeathState;

    private bool _fromFall;
    private float _pauseTimer = 0.2f;
    private float _animationTimer = 2.333f;
    private float _blackTimer = 1f;

    public GameDeathState(GameManager gameManager, bool fromFall) : base(gameManager)
    {
        _fromFall = fromFall;
    }

    public override void Enter()
    {
        GameManager.GameCanvas.gameObject.SetActive(false);
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
                    GameManager.Player.Mesh.transform.position = new Vector3(GameManager.Player.Mesh.transform.position.x, GameManager.Player.Mesh.transform.position.y, -7);
                    GameManager.BlackBackground.gameObject.SetActive(true);
                    CameraManager.Instance.ChangeCamera(GameManager.DeathAnimationCam);
                }
            }
        } else if (_currentDeathState == DeathState.Animation)
        {
            _animationTimer -= Time.deltaTime;

            GameManager.BlackBackground.material.SetFloat("_Fade", Mathf.Abs(_animationTimer * 2 / 2.333f - 2f));

            if (_animationTimer <= 0f)
            {
                GameManager.BlackScreen.gameObject.SetActive(true);
                _currentDeathState = DeathState.Black;
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
