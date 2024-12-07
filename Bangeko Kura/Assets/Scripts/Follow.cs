using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    private Transform _followTarget;

    public Transform FollowTarget { set { _followTarget = value; } }

    private void Update()
    {
        transform.position = new Vector2(_followTarget.position.x, _followTarget.position.y);
    }
}
