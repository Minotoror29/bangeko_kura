using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundCloud : Cloud
{
    public override void UpdateLogic()
    {
        transform.position += Direction * Speed * Time.deltaTime * Vector3.right;

        float viewportXPos = Camera.main.WorldToViewportPoint(transform.position).x;
        float spriteXExtents = Camera.main.WorldToViewportPoint(Camera.main.ViewportToWorldPoint(new Vector2(0, 0)) + SpriteRenderer.bounds.extents).x;

        if (viewportXPos < 0f - spriteXExtents || viewportXPos > 1f + spriteXExtents)
        {
            CloudsManager.RespawnBackgroundCloud(this);
        }

        //Debug.Log(gameObject.name + " | " + Camera.main.WorldToViewportPoint(SpriteRenderer.bounds.extents));
    }
}
