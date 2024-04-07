using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    private LineRenderer _lineRenderer;

    [SerializeField] private float lifeTime = 1f;
    private float _lifeTimer;

    private void Update()
    {
        UpdateLogic();
    }

    public void Initialize(Vector3 startPosition, Vector3 endPosition, float width)
    {
        _lineRenderer = GetComponent<LineRenderer>();

        _lineRenderer.SetPosition(0, new Vector3(startPosition.x, startPosition.y, -1));
        _lineRenderer.SetPosition(1, new Vector3(endPosition.x, endPosition.y, -1));

        _lineRenderer.startWidth = width;
        _lineRenderer.endWidth = width;

        _lifeTimer = 0f;
    }

    public void UpdateLogic()
    {
        if (_lifeTimer < lifeTime)
        {
            _lifeTimer += Time.deltaTime;
        } else
        {
            Destroy(gameObject);
        }
    }
}
