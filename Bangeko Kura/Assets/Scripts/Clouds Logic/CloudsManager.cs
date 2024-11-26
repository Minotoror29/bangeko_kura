using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudsManager : MonoBehaviour
{
    [SerializeField] private List<Cloud> foreGroundCloudPrefabs;
    [SerializeField] private List<Cloud> backgroundCloudPrefabs;
    [SerializeField] private float minX, maxX, minY, maxY;
    [SerializeField] private float minCloudSpeed, maxCloudSpeed;
    [SerializeField] private int cloudsAmount = 2;

    private List<Cloud> _clouds;

    private void Start()
    {
        _clouds = new();

        foreach (Cloud cloud in foreGroundCloudPrefabs)
        {
            for (int i = 0; i < cloudsAmount; i++)
            {
                Vector2 randomPosition = new(Random.Range(minX, maxX), Random.Range(minY, maxY));
                float randomSpeed = Random.Range(minCloudSpeed, maxCloudSpeed);

                Cloud newCloud = Instantiate(cloud, randomPosition, Quaternion.identity);
                newCloud.Initialize(this, randomSpeed, minX, maxX);
                _clouds.Add(newCloud);
            }
        }

        foreach (Cloud cloud in backgroundCloudPrefabs)
        {
            Vector2 randomPosition = new(Random.Range(0f, 1f), Random.Range(0f, 1f));
            float randomSpeed = Random.Range(minCloudSpeed, maxCloudSpeed);

            Cloud newCloud = Instantiate(cloud, Camera.main.ViewportToWorldPoint(randomPosition), Quaternion.identity, Camera.main.transform);
            newCloud.Initialize(this, randomSpeed, minX, maxX);
            _clouds.Add(newCloud);
        }
    }

    public void RespawnCloud(Cloud cloud)
    {
        float randomY = Random.Range(minY, maxY);
        float randomSpeed = Random.Range(minCloudSpeed, maxCloudSpeed);
        float xPos = minX - cloud.SpriteRenderer.bounds.extents.x;
        cloud.SetNewPosition(new Vector2(xPos, randomY), randomSpeed);
    }

    public void RespawnBackgroundCloud(BackgroundCloud cloud)
    {
        float randomY = Random.Range(0f, 1f);
        float randomSPeed = Random.Range(minCloudSpeed, maxCloudSpeed);
        float xPos = 0 - Camera.main.WorldToViewportPoint(Camera.main.ViewportToWorldPoint(new Vector2(0, 0)) + cloud.SpriteRenderer.bounds.extents).x;
        cloud.SetNewPosition(Camera.main.ViewportToWorldPoint(new Vector2(xPos, randomY)), randomSPeed);
    }

    public void UpdateLogic()
    {
        foreach (Cloud cloud in _clouds)
        {
            cloud.UpdateLogic();
        }
    }
}
