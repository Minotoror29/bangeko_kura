using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Altar : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;

    public UnityEvent OnActivation; 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerController>())
        {
            gameObject.SetActive(false);
            OnActivation?.Invoke();
        }
    }
}
