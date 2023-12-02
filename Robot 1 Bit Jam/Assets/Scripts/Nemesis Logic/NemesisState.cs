using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NemesisState : State
{
    private NemesisController _controller;

    private NewPlayerController _player;

    public NemesisController Controller { get { return _controller; } }
    public NewPlayerController Player { get { return _player; } }

    public NemesisState(NemesisController controller, NewPlayerController player)
    {
        _controller = controller;
        _player = player;
    }
}
