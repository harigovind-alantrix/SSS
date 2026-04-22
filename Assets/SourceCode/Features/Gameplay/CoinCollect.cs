using System.Collections;
using System.Collections.Generic;
using Infrastructure.Pooling;
using UnityEngine;
namespace Features.Gameplay
{
    public class CoinCollect : MonoBehaviour
    {
        public float rotateSpeed = 120f;

    private CoinPool pool;

    void Start()
    {
        pool = FindObjectOfType<CoinPool>();
    }

    void Update()
    {
        transform.Rotate(0f, rotateSpeed * Time.deltaTime, 0f);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (CoinManager.Instance != null)
                CoinManager.Instance.AddCoin(1);

            if (pool != null)
                pool.ReturnCoin(gameObject);
            else
                gameObject.SetActive(false);
        }
    }
    }
}

