using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicTrigger : MonoBehaviour
{
    private void Start()
    {
        MusicManager.Instance.PlayMusicLayer(MusicLayer.Drones, true);
    }
}
