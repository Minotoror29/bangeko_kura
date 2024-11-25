using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CliffParallax : MonoBehaviour
{
    [SerializeField] private float parallaxStartY;
    [SerializeField] private float parallaxEndY;

    private void Update()
    {
        if (Camera.main.transform.position.y < parallaxStartY && Camera.main.transform.position.y > parallaxEndY)
        {
            float yPosition = parallaxEndY - (Camera.main.transform.position.y - parallaxEndY);
            transform.position = new Vector2(transform.position.x, yPosition);
        }
    }
}
