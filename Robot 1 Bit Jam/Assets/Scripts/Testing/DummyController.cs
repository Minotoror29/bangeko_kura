using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyController : MonoBehaviour
{
    [SerializeField] private HealthSystem healthSystem;
    [SerializeField] private List<SkinnedMeshRenderer> meshRenderers;
    [SerializeField] private float changeColorTime = 0.1f;
    private float _changeColorTimer;

    private void Start()
    {
        healthSystem.Initialize(transform);
        healthSystem.OnDamage += TakeDamage;
        healthSystem.OnDeath += ResetHealth;
    }

    private void TakeDamage()
    {
        ChangeColor();
    }

    private void ChangeColor()
    {
        foreach (SkinnedMeshRenderer renderer in meshRenderers)
        {
            renderer.material.SetColor("_Dark_Color", Color.white);
        }

        _changeColorTimer = changeColorTime;
    }

    private void ResetHealth(HealthSystem healthSYstem, Transform deathSource)
    {
        healthSystem.ResetHealth();
    }

    private void Update()
    {
        if (_changeColorTimer > 0f)
        {
            _changeColorTimer -= Time.deltaTime;
        } else
        {
            foreach (SkinnedMeshRenderer renderer in meshRenderers)
            {
                renderer.material.SetColor("_Dark_Color", Color.black);
            }
        }
    }
}
