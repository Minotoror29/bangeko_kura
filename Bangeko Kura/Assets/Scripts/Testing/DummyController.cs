using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyController : MonoBehaviour
{
    [SerializeField] private Animator generalAnimator;

    [SerializeField] private HealthSystem healthSystem;
    [SerializeField] private bool indestructible = true;
    [SerializeField] private List<SkinnedMeshRenderer> meshRenderers;
    [SerializeField] private float changeColorTime = 0.1f;
    private float _changeColorTimer;

    [Header("Effects")]
    [SerializeField] private Scrap scrapPrefab;
    [SerializeField] private List<GameObject> explosionEffects;

    private void Start()
    {
        healthSystem.Initialize(transform);
        healthSystem.OnDamage += TakeDamage;
        if (indestructible)
        {
            healthSystem.OnDeath += ResetHealth;
        } else
        {
            healthSystem.OnDeath += Die;
        }
    }

    private void TakeDamage()
    {
        ChangeColor();
        Squish();
    }

    private void ChangeColor()
    {
        foreach (SkinnedMeshRenderer renderer in meshRenderers)
        {
            renderer.material.SetColor("_Dark_Color", Color.white);
        }

        _changeColorTimer = changeColorTime;
    }

    private void Squish()
    {
        generalAnimator.SetTrigger("Squish");
    }

    private void ResetHealth(HealthSystem healthSystem, Transform deathSource)
    {
        this.healthSystem.ResetHealth();
    }

    private void Die(HealthSystem healthSystem, Transform deathSource)
    {
        Scrap newScrap = Instantiate(scrapPrefab, transform.position, Quaternion.identity);
        newScrap.Initialize();

        int randomExplosion = Random.Range(0, explosionEffects.Count);
        float randomExplosionRotation = Random.Range(0f, 360f);
        float randomExplosionScale = Random.Range(0.75f, 1f);
        GameObject newExplosion = Instantiate(explosionEffects[randomExplosion], transform.position, Quaternion.Euler(0f, 0f, randomExplosionRotation));
        newExplosion.transform.localScale = new Vector3(randomExplosionScale, randomExplosionScale, 1f);
        Destroy(newExplosion, 0.367f);

        gameObject.SetActive(false);
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
