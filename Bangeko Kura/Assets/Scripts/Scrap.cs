using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scrap : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private List<Sprite> sprites;
    [SerializeField] private float lifetime = 5f;

    public void Initialize()
    {
        int randomSprite = Random.Range(0, sprites.Count);
        spriteRenderer.sprite = sprites[randomSprite];

        float randomRotation = Random.Range(0f, 360f);
        transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, randomRotation));

        float randomScale = Random.Range(0.75f, 1f);
        transform.localScale = new Vector3(randomScale, randomScale, 1f);

        Destroy(gameObject, lifetime);
    }
}
