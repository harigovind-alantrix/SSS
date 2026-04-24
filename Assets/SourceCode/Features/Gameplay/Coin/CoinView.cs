using System;
using UnityEngine;

namespace Features.Gameplay.Coin
{
    [RequireComponent(typeof(Collider))]
    public class CoinView : MonoBehaviour
    {
        public Action<CoinView> OnCollected;

        void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            OnCollected?.Invoke(this);
        }

        public void Tick(float rotateSpeed)
        {
            transform.Rotate(0f, rotateSpeed * Time.deltaTime, 0f);
        }
    }
}