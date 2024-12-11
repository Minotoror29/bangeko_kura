using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    [SerializeField] private float effectTime;

    private Animator _animator;
    private float _effectTimer;
    private bool _isPlaying = true;

    public void Initialize()
    {
        _animator = GetComponent<Animator>();
        _effectTimer = effectTime;
    }

    public void PlayEffect(bool play)
    {
        _isPlaying = play;

        if (_animator != null)
        {
            if (play)
            {
                _animator.speed = 1f;
            } else
            {
                _animator.speed = 0f;
            }
        }
    }

    private void Update()
    {
        if (!_isPlaying) return;

        if (_effectTimer > 0f)
        {
            _effectTimer -= Time.deltaTime;

            if (_effectTimer <= 0f)
            {
                Destroy(gameObject);
            }
        }
    }
}
