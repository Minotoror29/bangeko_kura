using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TutorialState { Inactive, Entering, Active, Exiting }

public class Tutorial : MonoBehaviour
{
    [SerializeField] private GameObject tutorialAnimation;
    [SerializeField] private List<SpriteRenderer> spriteRenderers;
    [SerializeField] private float fadeTime = 1f;

    private TutorialState _currentState = TutorialState.Inactive;
    private float _fadeTimer = 0f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _currentState = TutorialState.Entering;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _currentState = TutorialState.Exiting;
        }
    }

    private void Update()
    {
        if (_currentState == TutorialState.Entering)
        {
            if (_fadeTimer < fadeTime)
            {
                _fadeTimer += Time.deltaTime;

                SetMaterialsFade();

                if (_fadeTimer >= fadeTime)
                {
                    _fadeTimer = 1f;
                    SetMaterialsFade();
                    _currentState = TutorialState.Active;
                }
            }
        }

        if (_currentState == TutorialState.Exiting)
        {
            if (_fadeTimer > 0f)
            {
                _fadeTimer -= Time.deltaTime;

                SetMaterialsFade();

                if (_fadeTimer <= 0f)
                {
                    _fadeTimer = 0f;
                    SetMaterialsFade();
                    _currentState = TutorialState.Inactive;
                }
            }
        }
    }

    private void SetMaterialsFade()
    {
        foreach (SpriteRenderer renderer in spriteRenderers)
        {
            renderer.material.SetFloat("_Fade", _fadeTimer);
        }
    }
}
