using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordController : MonoBehaviour
{
    private PlayerController _player;

    [SerializeField] private int damage = 5;

    public void Initialize(PlayerController player)
    {
        _player = player;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out HealthSystem healthSystem))
        {
            healthSystem.TakeDamage(damage, _player.transform);
        }
    }
}
