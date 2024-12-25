using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SpawnPoint
{
    [SerializeField] private Transform spawnPosition;
    [SerializeField] private GameObject ground;

    public Transform SpawnPosition { get { return spawnPosition; } }
    public GameObject Ground { get {  return ground; } }
}
