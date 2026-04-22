using System.Collections;
using System.Collections.Generic;
using Infrastructure.Pooling;
using UnityEngine;
namespace Features.Gameplay
{
    public class CoinSpawner : MonoBehaviour
    {
        public CoinPool pool;

        [Range(0f, 1f)]
        public float coinChance = 0.45f;

        public void TrySpawnCoin(Vector3 platformPos)
        {
            if (pool == null) return;

            if (Random.value > coinChance)
                return;

            GameObject coin = pool.GetCoin();

            Vector3 spawnPos;

            if (Random.value > 0.5f)
            {
                // On platform
                spawnPos = platformPos + Vector3.up * 1.2f;
            }
            else
            {
                // In air near platform
                spawnPos = platformPos + new Vector3(
                    Random.Range(-2f, 2f),
                    Random.Range(1.5f, 3f),
                    0f);
            }

            coin.transform.position = spawnPos;
            coin.transform.rotation = Quaternion.identity;
        }
    }
}
