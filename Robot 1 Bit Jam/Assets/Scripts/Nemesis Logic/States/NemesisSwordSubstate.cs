using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NemesisSwordSubstate : NemesisState
{
    private NemesisSwordState _superstate;

    public NemesisSwordState Superstate { get { return _superstate; } }

    protected NemesisSwordSubstate(NemesisController controller, NemesisSwordState superstate) : base(controller)
    {
        _superstate = superstate;
    }
}
