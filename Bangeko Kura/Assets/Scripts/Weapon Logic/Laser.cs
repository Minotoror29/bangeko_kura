using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    private LineRenderer _lineRenderer;

    [SerializeField] private float lifeTime = 1f;
    private float _lifeTimer;

    [SerializeField] private Effect laserParticles;

    private Vector3 _startPosition;
    private Vector3 _endPosition;

    private void Update()
    {
        UpdateLogic();
    }

    public void Initialize(Vector3 startPosition, Vector3 endPosition, float width)
    {
        _startPosition = startPosition;
        _endPosition = endPosition;

        _lineRenderer = GetComponent<LineRenderer>();

        _lineRenderer.SetPosition(0, new Vector3(startPosition.x, startPosition.y, -1));
        _lineRenderer.SetPosition(1, new Vector3(endPosition.x, endPosition.y, -1));

        _lineRenderer.startWidth = width;
        _lineRenderer.endWidth = width;

        _lifeTimer = lifeTime;
    }

    public void UpdateLogic()
    {
        if (_lifeTimer > 0f)
        {
            _lifeTimer -= Time.deltaTime;

            _lineRenderer.widthMultiplier = _lifeTimer * (1 / lifeTime);
        } else
        {
            Vector3 laserDirection = (_endPosition - _startPosition).normalized;
            float laserLength = (_endPosition - _startPosition).magnitude;
            for (int i = 0; i < laserLength; i++)
            {
                Effect newParticle = Instantiate(laserParticles, _startPosition + laserDirection * i, Quaternion.LookRotation(_endPosition, Vector3.back));
                newParticle.Initialize();
            }
            Destroy(gameObject);
        }
    }
}
