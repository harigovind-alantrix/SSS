using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessPlatformGenerator : MonoBehaviour
{

    // public PlatformPool pool;

    // private Vector3 lastPlatformPos;

    // [Header("Horizontal")]
    // public float minXStep = 2f;
    // public float maxXStep = 4f;

    // [Header("Vertical")]
    // public float minYStep = 3f;  // 🔥 important (no head hit)
    // public float maxYStep = 6f;

    // [Header("Spawn")]
    // public int initialPlatforms = 6;

    // private int direction = 1; // 🔥 controls zig-zag

    // void Start()
    // {
    //     lastPlatformPos = Vector3.zero;

    //     for (int i = 0; i < initialPlatforms; i++)
    //     {
    //         SpawnPlatform();
    //     }
    // }

    // void Update()
    // {
    //     if (lastPlatformPos.y < transform.position.y + 15f)
    //     {
    //         SpawnPlatform();
    //     }
    // }

    // void SpawnPlatform()
    // {
    //     GameObject platform = pool.GetPlatform();

    //     // 🔥 Alternate direction (left/right)
    //     direction *= -1;

    //     float xStep = Random.Range(minXStep, maxXStep) * direction;
    //     float yStep = Random.Range(minYStep, maxYStep);

    //     // 🔥 Occasionally create harder jump
    //     if (Random.value < 0.25f)
    //     {
    //         yStep += 2f; // higher jump
    //     }

    //     Vector3 newPos = new Vector3(
    //         lastPlatformPos.x + xStep,
    //         lastPlatformPos.y + yStep,
    //         0f
    //     );

    //     platform.transform.position = newPos;

    //     lastPlatformPos = newPos;
    // }
    public PlatformPool pool;

    private float currentY = 0f;

    [Header("Layer Settings")]
    public float minYStep = 3f;
    public float maxYStep = 6f;

    [Header("Horizontal Spread")]
    public float minX = -5f;
    public float maxX = 5f;

    [Header("Platforms Per Layer")]
    public int minPlatforms = 2;
    public int maxPlatforms = 4;

    public int initialLayers = 5;
    public float minVerticalClearance = 4f;
    void Start()
    {
        for (int i = 0; i < initialLayers; i++)
        {
            SpawnLayer();
        }
    }

    void Update()
    {
        if (currentY < transform.position.y + 15f)
        {
            SpawnLayer();
        }
    }

    void SpawnLayer()
    {
        float yStep = Random.Range(minYStep, maxYStep);

        // 🔥 FORCE MINIMUM HEIGHT (NO OVERLAP EVER)
        yStep = Mathf.Max(yStep, minVerticalClearance);

        currentY += yStep;

        int count = Random.Range(minPlatforms, maxPlatforms + 1);

        List<float> usedXPositions = new List<float>();

        for (int i = 0; i < count; i++)
        {
            GameObject platform = pool.GetPlatform();

            float xPos;
            int attempts = 0;

            do
            {
                xPos = Random.Range(minX, maxX);
                attempts++;
            }
            while (IsTooClose(xPos, usedXPositions) && attempts < 10);

            usedXPositions.Add(xPos);

            platform.transform.position = new Vector3(xPos, currentY, 0f);
        }
    }
    bool IsTooClose(float newX, List<float> existingX)
    {
        float minDistance = 2.5f; // 🔥 tweak this

        foreach (float x in existingX)
        {
            if (Mathf.Abs(newX - x) < minDistance)
                return true;
        }

        return false;
    }
}
