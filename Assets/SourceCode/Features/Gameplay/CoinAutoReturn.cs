using System.Collections;
using System.Collections.Generic;
using Infrastructure.Pooling;
using UnityEngine;
namespace Features.Gameplay
{
    public class CoinAutoReturn : MonoBehaviour
    {
        public float returnDistanceBelowPlayer = 20f;

        private CoinPool pool;
        private EndlessPlatformGenerator generator;

        void Start()
        {
            pool = FindObjectOfType<CoinPool>();
            generator = FindObjectOfType<EndlessPlatformGenerator>();
        }

        void Update()
        {
            if (pool == null || generator == null || generator.targetPlayer == null)
                return;

            float playerY = generator.targetPlayer.position.y;

            if (transform.position.y < playerY - returnDistanceBelowPlayer)
            {
                pool.ReturnCoin(gameObject);
            }
        }
    }
}

