using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    private Transform _controller;

    public Transform Controller { get { return _controller; } }

    public virtual void Initialize(Transform controller)
    {
        _controller = controller;
    }

    public virtual void UpdateLogic()
    {

    }
}
