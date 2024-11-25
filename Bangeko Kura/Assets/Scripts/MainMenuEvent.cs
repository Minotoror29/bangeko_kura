using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class MainMenuEvent
{
    [SerializeField] private float time;
    [SerializeField] private UnityEvent events;

    public float Time { get { return time; } }
    public UnityEvent Events { get { return events; } }
}
