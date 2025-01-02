using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Altar : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;

    private EventInstance _altarSound;

    public UnityEvent OnActivation;

    private void Start()
    {
        _altarSound = RuntimeManager.CreateInstance("event:/Environment/Interaction Autel");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerController>())
        {
            _altarSound.start();
            GetComponent<BoxCollider2D>().enabled = false;
            OnActivation?.Invoke();
        }
    }
}
