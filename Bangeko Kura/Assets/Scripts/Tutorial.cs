using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    [SerializeField] private GameObject tutorialAnimation;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        tutorialAnimation.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        tutorialAnimation.SetActive(false);
    }
}
