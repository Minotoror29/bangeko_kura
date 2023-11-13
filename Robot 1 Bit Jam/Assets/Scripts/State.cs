using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
    public abstract void Enter();
    public abstract void Exit();
    public abstract void UpdateLogic();
    public abstract void UpdatePhysics();
}
