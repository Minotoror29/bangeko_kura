using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;

    private CloudsManager _cloudsManager;
    private float _speed;
    private float _minX, _maxX;

    public SpriteRenderer SpriteRenderer { get { return spriteRenderer; } }
    public CloudsManager CloudsManager { get { return _cloudsManager; } }
    public float Speed { get { return _speed; } }

    public void Initialize(CloudsManager cloudsManager,float speed, float minX, float maxX)
    {
        _cloudsManager = cloudsManager;
        _speed = speed;
        _minX = minX;
        _maxX = maxX;
    }

    public void SetNewPosition(Vector2 newPosition, float speed)
    {
        transform.position = newPosition;
        _speed = speed;
    }

    public virtual void UpdateLogic()
    {        
        transform.position += _speed * Time.deltaTime * Vector3.right;

        if (transform.position.x < _minX - spriteRenderer.bounds.extents.x || transform.position.x > _maxX + spriteRenderer.bounds.extents.x)
        {
            _cloudsManager.RespawnCloud(this);
        }
    }
}
