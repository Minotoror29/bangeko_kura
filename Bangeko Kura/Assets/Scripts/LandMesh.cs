using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandMesh : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private List<SkinnedMeshRenderer> meshRenderers;
    [SerializeField] private float changeColorTime = 0.1f;
    [SerializeField] private Color lightColor = Color.white;
    [SerializeField] private Color darkColor = Color.black;

    private float _changeColorTimer = 0f;

    public void TakeDamage()
    {
        animator.SetTrigger("Squish");
        ChangeColor();
    }

    private void ChangeColor()
    {
        foreach (SkinnedMeshRenderer renderer in meshRenderers)
        {
            renderer.material.SetColor("_Dark_Color", lightColor);
        }

        _changeColorTimer = changeColorTime;
    }

    public void UpdateLogic()
    {
        if (_changeColorTimer > 0f)
        {
            _changeColorTimer -= Time.deltaTime;
        }
        else
        {
            foreach (SkinnedMeshRenderer renderer in meshRenderers)
            {
                renderer.material.SetColor("_Dark_Color", darkColor);
            }
        }
    }
}
