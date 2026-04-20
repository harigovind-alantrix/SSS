using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessPlatformGenerator : MonoBehaviour
{
    public PlatformPool pool;
    [Header("Target Player")]
    public Transform targetPlayer;

    [Header("Start")]
    public int initialPlatforms = 12;

    [Header("Jump Distances")]
    public float minXStep = 1.5f;
    public float maxXStep = 4f;

    public float minYStep = 2.5f;
    public float maxYStep = 4.5f;

    [Header("Bounds")]
    public float minX = -5f;
    public float maxX = 5f;

    [Header("Spawn Ahead")]
    public float spawnAheadY = 15f;

    private float lastX = 0f;
    private float lastY = 0f;

    void Start()
    {
        for (int i = 0; i < initialPlatforms; i++)
            SpawnNextPlatform();
    }

    void Update()
    {
        if (targetPlayer == null) return;
        if (lastY < targetPlayer.position.y + spawnAheadY)
        {
            SpawnNextPlatform();
        }
    }

    void SpawnNextPlatform()
    {
        float yStep = Random.Range(minYStep, maxYStep);

        // choose left or right movement
        float dir = Random.value > 0.5f ? 1f : -1f;
        float xStep = Random.Range(minXStep, maxXStep) * dir;

        float newX = Mathf.Clamp(lastX + xStep, minX, maxX);
        float newY = lastY + yStep;

        // avoid straight vertical blocking
        if (Mathf.Abs(newX - lastX) < 1f)
        {
            newX += dir * 1.5f;
            newX = Mathf.Clamp(newX, minX, maxX);
        }

        GameObject platform = pool.GetPlatform();
        platform.transform.position = new Vector3(newX, newY, 0f);

        lastX = newX;
        lastY = newY;

    }
    public void SetTarget(Transform newTarget)
    {
        targetPlayer = newTarget;
    }
}
