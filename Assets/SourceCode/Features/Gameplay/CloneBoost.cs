using Cysharp.Threading.Tasks;
using Data.Configs;
using UnityEngine;

namespace Features.Gameplay
{
    public class CloneBoost
    {
        private readonly Transform _transform;
        private readonly PlayerConfig _config;

        public CloneBoost(Transform transform, PlayerConfig config)
        {
            _transform = transform;
            _config    = config;
        }

        public void ApplyBoost(Vector3 direction)
        {
            BoostAsync(direction).Forget();
        }

        private async UniTaskVoid BoostAsync(Vector3 direction)
        {
            float timer = 0f;

            while (timer < _config.boostDuration)
            {
                Vector3 boostMove;

                if (direction == Vector3.zero)
                {
                    boostMove = Vector3.up * (_config.upwardForce * 1.6f) * Time.deltaTime;
                }
                else
                {
                    boostMove = (direction.normalized * _config.boostForce
                                 + Vector3.up * _config.upwardForce) * Time.deltaTime;
                }

                _transform.position += boostMove;

                timer += Time.deltaTime;
                await UniTask.Yield();
            }
        }
    }
}