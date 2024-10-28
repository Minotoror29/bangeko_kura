using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneFrame : MonoBehaviour
{
    [SerializeField] private float frameTime = 2f;

    public float FrameTime { get { return frameTime; } }
}
