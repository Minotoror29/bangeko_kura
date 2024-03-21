using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HealthDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI health;
    [SerializeField] private HealthSystem healthSystem;

    private void Update()
    {
        health.text = healthSystem.CurrentHealth.ToString();
    }
}
