using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CreditsState { Inactive, Active, Over }

public class CreditsSection : MonoBehaviour
{
    [SerializeField] private float activeTime = 3f;
    [SerializeField] private List<Animator> elements;

    private CreditsState _currentState;
    private float _timer;

    private void Start()
    {
        Initialize();
    }

    private void Update()
    {
        Updatelogic();
    }

    public void Initialize()
    {
        _currentState = CreditsState.Inactive;
        _timer = activeTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_currentState == CreditsState.Inactive)
        {
            _currentState = CreditsState.Active;
            foreach (Animator element in elements)
            {
                element.CrossFade("Credits_Enter", 0f);
            }
        }
    }

    public void Updatelogic()
    {
        if (_currentState == CreditsState.Active)
        {
            if (_timer > 0f)
            {
                _timer -= Time.deltaTime;

                if (_timer <= 0f)
                {
                    _currentState = CreditsState.Over;
                    foreach (Animator element in elements)
                    {
                        element.CrossFade("Credits_Exit", 0f);
                    }
                }
            }
        }
    }
}
