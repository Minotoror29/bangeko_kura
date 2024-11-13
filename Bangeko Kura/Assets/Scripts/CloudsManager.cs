using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudsManager : MonoBehaviour
{
    [SerializeField] private List<Cloud> cloudPrefabs;
    [SerializeField] private float minX, maxX, minY, maxY;
    [SerializeField] private float minCloudSpeed, maxCloudSpeed;

    private List<Cloud> _clouds;

    private void Start()
    {
        _clouds = new();

        foreach (Cloud cloud in cloudPrefabs)
        {
            Vector2 randomPosition = new(Random.Range(minX, maxX), Random.Range(minY, maxY));
            float randomDirection = Random.Range(0, 2) * 2 - 1;
            float randomSpeed = Random.Range(minCloudSpeed, maxCloudSpeed);

            Cloud newCloud =  Instantiate(cloud, randomPosition, Quaternion.identity);
            newCloud.Initialize(this, randomDirection, randomSpeed, minX, maxX);
            _clouds.Add(newCloud);
        }
    }

    public void RespawnCloud(Cloud cloud)
    {
        float randomY = Random.Range(minY, maxY);
        float randomDirection = Random.Range(0, 2) * 2 - 1;
        float randomSpeed = Random.Range(minCloudSpeed, maxCloudSpeed);

        if (randomDirection == 1)
        {
            float xPos = minX - cloud.SpriteRenderer.bounds.extents.x;
            cloud.SetNewPosition(new Vector2(xPos, randomY), randomSpeed, randomDirection);
        } else if (randomDirection == -1)
        {
            float xPos = maxX + cloud.SpriteRenderer.bounds.extents.x;
            cloud.SetNewPosition(new Vector2(xPos, randomY), randomSpeed, randomDirection);
        }
    }

    private void Update()
    {
        foreach (Cloud cloud in _clouds)
        {
            cloud.UpdateLogic();
        }
    }
}
