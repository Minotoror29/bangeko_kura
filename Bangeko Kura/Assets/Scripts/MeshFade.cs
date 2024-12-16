using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshFade : MonoBehaviour
{
    [SerializeField] private List<Renderer> meshRenderers;
    [SerializeField] private float startFadeTime;
    [SerializeField] private float fadeTime;

    private float _timer = 0f;

    private void Update()
    {
        if (_timer < startFadeTime)
        {
            _timer += Time.deltaTime;

            
        } else if (_timer >= startFadeTime && _timer < startFadeTime + fadeTime)
        {
            _timer += Time.deltaTime;

            foreach (Renderer renderer in meshRenderers)
            {
                renderer.material.SetFloat("_Fade", Mathf.Abs((_timer - startFadeTime) / fadeTime - 1));
            }

            if (_timer >= startFadeTime + fadeTime)
            {
                Destroy(gameObject);
            }
        }
    }
}
