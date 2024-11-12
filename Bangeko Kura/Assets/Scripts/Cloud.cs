using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    private float _direction;
    private float _speed;

    public void Initialize(float direction, float speed)
    {
        _direction = direction;
        _speed = speed;
    }

    public void UpdateLogic()
    {
        float xPos = _direction * _speed;
        
        transform.position += Vector3.right * xPos * Time.deltaTime;
    }
}
