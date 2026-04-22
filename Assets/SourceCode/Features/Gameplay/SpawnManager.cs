using System;
using System.Collections.Generic;
using Cinemachine;
using Core.Interfaces;
using Core.Messages.Gameplay;
using Core.Messages.System;
using Core.Models;
using Data.Configs;
using Entities.Player;
using MessagePipe;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Features.Gameplay
{
    public class SpawnManager : IInitializable, IDisposable
    {
        private readonly IObjectResolver _container;
        private readonly ISubscriber<OnGameStateChanged> _gameStateSubscriber;
        private readonly ISubscriber<CloneInputEvent> _cloneSubscriber;
        private readonly IShopService _shopService;
        private readonly CinemachineVirtualCamera _vcam;
        private readonly PlayerConfig _config;
        private readonly Transform _spawnPoint;

        private GameObject _currentPlayer;
        private GameObject _lastPlayer;
        private IDisposable _subscription;

        public SpawnManager(
            IObjectResolver container,
            ISubscriber<OnGameStateChanged> gameStateSubscriber,
            ISubscriber<CloneInputEvent> cloneSubscriber,
            IShopService shopService,
            CinemachineVirtualCamera vcam,
            PlayerConfig config,
            [Key(InjectId.SpawnPoint)] Transform spawnPoint)
        {
            _container = container;
            _gameStateSubscriber = gameStateSubscriber;
            _cloneSubscriber = cloneSubscriber;
            _shopService = shopService;
            _vcam = vcam;
            _config = config;
            _spawnPoint = spawnPoint;
        }

        public void Initialize()
        {
            var disposableBag = DisposableBag.CreateBuilder();
            _gameStateSubscriber.Subscribe(OnGameStateChanged).AddTo(disposableBag);
            _cloneSubscriber.Subscribe(OnCloneInput).AddTo(disposableBag);
            _subscription = disposableBag.Build();
        }

        private void OnGameStateChanged(OnGameStateChanged message)
        {
            if (message.CurrentState == GameState.Playing)
            {
                if (_currentPlayer != null) GameObject.Destroy(_currentPlayer);
                if (_lastPlayer != null) GameObject.Destroy(_lastPlayer);
                SpawnPlayer(_spawnPoint.position, _spawnPoint.rotation);
            }
        }

        private void OnCloneInput(CloneInputEvent evt)
        {
            if (_currentPlayer == null)
            {
                Debug.LogWarning("[CloneManager] No current player.");
                return;
            }

            PlayerController pc = _currentPlayer.GetComponent<PlayerController>();
            if (pc == null)
            {
                Debug.LogWarning("[CloneManager] PlayerController missing.");
                return;
            }

            Vector3 intendedDirection = Mathf.Abs(evt.Axis) > 0.1f
                ? new Vector3(evt.Axis, 0f, 0f).normalized
                : Vector3.zero;

            Vector3[] directionsToTry = GetDirectionPriority(intendedDirection);

            Vector3 spawnPos = Vector3.zero;
            Vector3 chosenDirection = Vector3.zero;
            bool foundSpot = false;

            foreach (Vector3 dir in directionsToTry)
            {
                if (TryGetSpawnPosition(_currentPlayer.transform.position, dir, out spawnPos))
                {
                    chosenDirection = dir;
                    foundSpot = true;
                    break;
                }
            }

            if (!foundSpot)
            {
                Debug.Log("[CloneManager] No valid spawn direction found — clone blocked.");
                return;
            }

            Quaternion spawnRot = _currentPlayer.transform.rotation;

            pc.Freeze();
            if (_lastPlayer != null)
            {
                GameObject.Destroy(_lastPlayer);
            }

            _lastPlayer = _currentPlayer;
            SpawnPlayer(spawnPos, spawnRot);

            Rigidbody cloneRb = _currentPlayer.GetComponent<Rigidbody>();
            var boost = new CloneBoost(cloneRb, _config);
            boost.ApplyBoost(chosenDirection);
        }

        //! Helpers

        private void SpawnPlayer(Vector3 position, Quaternion rotation)
        {
            var prefab = _shopService.GetSelected().prefab ?? _config.playerPrefab;
            _currentPlayer = GameObject.Instantiate(prefab, position, rotation);

            Rigidbody rb = _currentPlayer.GetComponent<Rigidbody>();
            if (rb != null)
                rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

            _container.InjectGameObject(_currentPlayer);

            if (_vcam != null)
                _vcam.Follow = _currentPlayer.transform;

            EndlessPlatformGenerator generator = GameObject.FindObjectOfType<EndlessPlatformGenerator>();
            if (generator != null)
                generator.SetTarget(_currentPlayer.transform);

            Debug.Log($"Player spawned at {position}");
        }

        private Vector3[] GetDirectionPriority(Vector3 intended)
        {
            Vector3[] allDirections =
            {
                Vector3.up,
                Vector3.right,
                Vector3.left,
                Vector3.forward,
                Vector3.back
            };

            if (intended == Vector3.zero)
                return allDirections;

            Vector3 opposite = -intended;

            var priority = new List<Vector3>();
            priority.Add(Vector3.up);
            priority.Add(intended);
            priority.Add(opposite);

            foreach (Vector3 dir in allDirections)
            {
                if (dir != Vector3.up && dir != intended && dir != opposite)
                    priority.Add(dir);
            }

            return priority.ToArray();
        }

        private const float CubeHalfExtent = 0.5f;
        private const float SpawnClearance = 0.05f;
        private static readonly int GeometryMask = LayerMask.GetMask("Ground", "Platform");

        private bool TryGetSpawnPosition(Vector3 playerPos, Vector3 direction, out Vector3 result)
        {
            float lateralShift = (direction != Vector3.zero && direction != Vector3.up)
                ? CubeHalfExtent * 1.2f
                : 0f;

            Vector3 candidate = direction == Vector3.up
                ? playerPos + Vector3.up * (CubeHalfExtent * 2f + SpawnClearance)
                : playerPos + direction * lateralShift;

            Vector3 probeOrigin = candidate + Vector3.up * 0.1f;

            float floorY;

            if (direction == Vector3.up)
            {
                floorY = playerPos.y + CubeHalfExtent;
            }
            else if (Physics.SphereCast(new Ray(probeOrigin, Vector3.down),
                         CubeHalfExtent, out RaycastHit floorHit, 20f, GeometryMask))
            {
                floorY = floorHit.point.y;
            }
            else
            {
                floorY = candidate.y - CubeHalfExtent;
            }

            Vector3 floorSurface = new Vector3(candidate.x, floorY + CubeHalfExtent, candidate.z);

            float ceilingY;

            if (Physics.SphereCast(new Ray(floorSurface, Vector3.up),
                    CubeHalfExtent, out RaycastHit ceilHit, 20f, GeometryMask))
            {
                ceilingY = ceilHit.point.y;
            }
            else
            {
                ceilingY = float.MaxValue;
            }

            float gap = ceilingY - floorY;
            float requiredGap = CubeHalfExtent * 2f + SpawnClearance * 2f;

            if (gap < requiredGap)
            {
                result = Vector3.zero;
                return false;
            }

            Vector3 spawnCenter =
                new Vector3(candidate.x, floorY + CubeHalfExtent + SpawnClearance, candidate.z);

            Vector3 halfExtents = Vector3.one * (CubeHalfExtent - 0.01f);

            Collider[] overlaps = Physics.OverlapBox(
                spawnCenter, halfExtents, Quaternion.identity);

            foreach (Collider col in overlaps)
            {
                if (_currentPlayer != null &&
                    col.transform.IsChildOf(_currentPlayer.transform))
                    continue;

                result = Vector3.zero;
                return false;
            }

            result = spawnCenter;
            return true;
        }

        public void Dispose()
        {
            _subscription?.Dispose();
        }
    }
}