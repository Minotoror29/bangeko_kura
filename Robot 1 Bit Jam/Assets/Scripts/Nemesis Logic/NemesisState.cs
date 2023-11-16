using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NemesisState : State
{
    private NemesisController _controller;

    private PlayerController _player;

    public NemesisController Controller { get { return _controller; } }
    public PlayerController Player { get { return _player; } }

    public NemesisState(NemesisController controller, PlayerController player)
    {
        _controller = controller;
        _player = player;
    }
}
