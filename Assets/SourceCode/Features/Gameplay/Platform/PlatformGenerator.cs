using System;
using System.Collections;
using System.Collections.Generic;
using Core.Messages.System;
using Features.Gameplay;
using MessagePipe;
using UnityEngine;
using VContainer;
using Random = UnityEngine.Random;

public class PlatformGenerator : MonoBehaviour
{
    public PlatformPool pool;
    [Header("Target Player")]
    private Transform targetPlayer;

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
    public CoinSpawner coinspawner;
    private IDisposable _subscription;

    [Inject]
    public void Construct(ISubscriber<OnGameRestarted> restartSubscriber)
    {
        _subscription = restartSubscriber.Subscribe(OnGameRestarted);
    }
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
    private void OnGameRestarted(OnGameRestarted _)
    {
        // Return all active platforms to pool
        foreach (var platform in FindObjectsOfType<PlatformAutoReturn>())
            pool.ReturnPlatform(platform.gameObject);

        // Reset state
        lastX = 0f;
        lastY = 0f;

        SpawnInitialPlatforms();
    }
    private void SpawnInitialPlatforms()
    {
        for (int i = 0; i < initialPlatforms; i++)
            SpawnNextPlatform();
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

        if (coinspawner != null)
        {
            coinspawner.TrySpawnCoin(platform.transform.position);
        }

        lastX = newX;
        lastY = newY;

    }
    public void SetTarget(Transform newTarget)
    {
        targetPlayer = newTarget;
    }
}
