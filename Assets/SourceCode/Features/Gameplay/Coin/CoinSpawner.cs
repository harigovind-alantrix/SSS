using Features.Gameplay.Coin;
using UnityEngine;
using VContainer;

namespace Features.Gameplay
{

    public class CoinSpawner : MonoBehaviour
    {
        [Range(0f, 1f)]
        public float coinChance = 0.45f;

        private CoinSystem _coinSystem;

        [Inject]
        public void Construct(CoinSystem coinSystem) => _coinSystem = coinSystem;

        public void TrySpawnCoin(Vector3 platformPos)
        {
            if (_coinSystem == null || Random.value > coinChance) return;

            Vector3 spawnPos = Random.value > 0.5f
                ? platformPos + Vector3.up * 1.2f
                : platformPos + new Vector3(
                    Random.Range(-2f, 2f),
                    Random.Range(1.5f, 3f),
                    0f);

            _coinSystem.Spawn(spawnPos);
        }
    }
}