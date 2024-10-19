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

        if (_bars != null)
        {
            ClearHealthDisplay();
        }
        _bars = new();

        for (int i = 0; i < healthSystem.CurrentHealth; i++)
        {
            GameObject newBar = Instantiate(barPrefab, barsParent);
            _bars.Add(newBar);
        }
    }

    private void ClearHealthDisplay()
    {
        for (int i = 0; i < _bars.Count; i++)
        {
            Destroy(_bars[i]);
        }

        _bars.Clear();
    }

    public void TakeDamage(int amount)
    {
        if (_bars.Count == 0) return;

        for (int i = 0; i < amount; i++)
        {
            GameObject barToRemove = _bars[^1];
            _bars.Remove(barToRemove);
            barToRemove.GetComponent<Animator>().CrossFade("TakeDamage", 0f);
            //barToRemove.SetActive(false);

            if (_bars.Count == 0) break;
        }
    }
}
