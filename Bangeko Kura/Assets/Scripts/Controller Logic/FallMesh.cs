using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallMesh : MonoBehaviour
{
    [SerializeField] private Transform mesh;

    public void Initialize(Vector2 position, Quaternion meshRotation)
    {
        transform.position = position;
        mesh.localRotation = meshRotation;
    }
}
