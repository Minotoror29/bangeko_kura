using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HealthDisplay : MonoBehaviour
{
    private HealthSystem _healthSystem;

    [SerializeField] private GameObject barPrefab;
    [SerializeField] private Transform barsParent;
    private List<GameObject> _bars;

    public void Initialize(HealthSystem healthSystem)
    {
        _healthSystem = healthSystem;
        _healthSystem.OnDamage += TakeDamage;

        _bars = new();
        for (int i = 0; i < healthSystem.CurrentHealth; i++)
        {
            GameObject newBar = Instantiate(barPrefab, barsParent);
            _bars.Add(newBar);
        }
    }

    public void TakeDamage(int amount)
    {
        if (_bars.Count == 0) return;

        for (int i = 0; i < amount; i++)
        {
            GameObject barToRemove = _bars[^1];
            _bars.Remove(barToRemove);
            Destroy(barToRemove);

            if (_bars.Count == 0) break;
        }
    }
}
