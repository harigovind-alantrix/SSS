using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Features.Gameplay
{
    public class CoinManager : MonoBehaviour
    {
        public static CoinManager Instance;

        public int coins = 0;
        void Awake()
        {
            Instance = this;
        }

        public void AddCoin(int amount)
        {
            coins += amount;
        }

    }
}

