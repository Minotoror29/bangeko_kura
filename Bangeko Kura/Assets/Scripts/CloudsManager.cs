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
            newCloud.Initialize(randomDirection, randomSpeed);
            _clouds.Add(newCloud);
        }
    }

    private void Update()
    {
        //A faire: faire disparaitre les nuages lorsqu'ils atteignent la position max et les faire réapparaitre à un endroit aléatoire en dehors des positions max

        foreach (Cloud cloud in _clouds)
        {
            cloud.UpdateLogic();
        }
    }
}
