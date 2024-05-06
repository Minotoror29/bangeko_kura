using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHeadController : PlayerController
{
    public override void Move(Vector2 direction, float speed)
    {
        base.Move(direction, speed);

        Mesh.Rotate(new Vector2(direction.y, -direction.x) * (speed / 10), Space.World);
    }

    public override void RotateMesh()
    {
    }

    public override void RotateMeshSmooth(Vector3 direction, float speed)
    {
    }
}
