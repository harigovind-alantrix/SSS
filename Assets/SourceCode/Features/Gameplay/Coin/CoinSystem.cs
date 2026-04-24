using System;
using System.Collections.Generic;
using Core.Interfaces;
using Core.Models;
using UnityEngine;
using Utils;
using VContainer;
using VContainer.Unity;

namespace Features.Gameplay.Coin
{
    public class CoinSystem : ITickable, IDisposable
    {
        private readonly IProgressionService _progression;
        private readonly ObjectPool<CoinView> _pool;
        private readonly List<CoinView> _active = new();

        private Transform _playerTarget;

        private const float RotateSpeed = 120f;
        private const float CullDistanceBelow = 20f;


        public CoinSystem(
            IObjectResolver container,
            [Key(InjectId.CoinView)] CoinView coinPrefab,
            [Key(InjectId.CoinPoolParent)] Transform poolParent,
            IProgressionService progression)
        {
            _progression = progression;

            _pool = new ObjectPool<CoinView>(
                container: container,
                prefab: coinPrefab,
                parent: poolParent,
                initialSize: 30,
                poolName: "[CoinPool]",
                maxSize: 100);
        }

        public void SetPlayerTarget(Transform player) => _playerTarget = player;

        public void Spawn(Vector3 position)
        {
            var coin = _pool.Get();
            coin.transform.SetPositionAndRotation(position, Quaternion.identity);
            coin.OnCollected = OnCoinCollected;
            _active.Add(coin);
        }

        public void Tick()
        {
            for (int i = _active.Count - 1; i >= 0; i--)
            {
                var coin = _active[i];

                if (coin == null)
                {
                    _active.RemoveAt(i);
                    continue;
                }

                coin.Tick(RotateSpeed);

                if (ShouldCull(coin))
                    ReturnAt(i);
            }
        }

        private bool ShouldCull(CoinView coin)
            => _playerTarget != null &&
               coin.transform.position.y < _playerTarget.position.y - CullDistanceBelow;

        private void OnCoinCollected(CoinView coin)
        {
            _progression.AddCoins(1);
            int index = _active.IndexOf(coin);
            if (index >= 0) ReturnAt(index);
        }

        private void ReturnAt(int index)
        {
            var coin = _active[index];
            _active.RemoveAt(index);
            coin.OnCollected = null;
            _pool.Return(coin);
        }

        public void Dispose() => _pool.Dispose();
    }
}