using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Altar : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private Collider2D coll;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<NewPlayerController>())
        {
            coll.enabled = false;
            gameManager.ChangeState(new GameCutsceneState(gameManager));
        }
    }
}
