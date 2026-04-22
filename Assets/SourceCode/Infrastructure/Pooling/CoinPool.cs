using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Infrastructure.Pooling
{
    public class CoinPool : MonoBehaviour
    {
        public GameObject coinPrefab;
        public int startAmount = 30;

        private Queue<GameObject> pool = new Queue<GameObject>();

        void Start()
        {
            for (int i = 0; i < startAmount; i++)
            {
                GameObject coin = Instantiate(coinPrefab, transform);
                coin.SetActive(false);
                pool.Enqueue(coin);
            }
        }

        public GameObject GetCoin()
        {
            GameObject coin;

            if (pool.Count > 0)
                coin = pool.Dequeue();
            else
                coin = Instantiate(coinPrefab, transform);

            coin.SetActive(true);
            return coin;
        }

        public void ReturnCoin(GameObject coin)
        {
            coin.SetActive(false);
            pool.Enqueue(coin);
        }
    }
}

