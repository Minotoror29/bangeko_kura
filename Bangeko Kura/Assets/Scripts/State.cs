using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
    public abstract void Enter();
    public abstract void Exit();
    public abstract void UpdateLogic();
    public abstract void UpdatePhysics();
    public abstract void OnCollisionEnter(Collision2D collision);
    public abstract void OnCollisionStay(Collision2D collision);
}
