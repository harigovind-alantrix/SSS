using System;
using UnityEngine;
using Cinemachine;
using VContainer;
using MessagePipe;
using Core.Models;
using Entities.Player;
using Core.Messages.Gameplay;
using Data.Configs;
using VContainer.Unity;

namespace Features.Gameplay
{
    public class CloneManager : IInitializable, IDisposable
    {
        private readonly CinemachineVirtualCamera _vcam;
        private readonly GameObject _clonePrefab;
        private readonly IObjectResolver _container;
        private readonly ISubscriber<CloneInputEvent> _cloneSubscriber;
        private readonly PlayerConfig _config;

        private GameObject _currentPlayer;
        private GameObject _lastPlayer;

        private IDisposable _subscription;
        
        [Inject]
        public CloneManager(
            ISubscriber<CloneInputEvent> cloneSubscriber,
            CinemachineVirtualCamera vcam,
            IObjectResolver container,
            PlayerConfig config,
            [Key(InjectId.Player)] GameObject initialPlayer,
            [Key(InjectId.CubePrefab)] GameObject clonePrefab)
        {
            _cloneSubscriber = cloneSubscriber;
            _vcam = vcam;
            _container = container;
            _config = config;
            _currentPlayer = initialPlayer;
            _clonePrefab = clonePrefab;
        }

        public void Initialize()
        {
            _subscription = _cloneSubscriber.Subscribe(OnCloneInput);

            if (_vcam != null && _currentPlayer != null)
            {
                _vcam.Follow = _currentPlayer.transform;
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

            Vector3 direction = Mathf.Abs(evt.Axis) > 0.1f
                ? new Vector3(evt.Axis, 0f, 0f).normalized
                : Vector3.zero;

            Vector3 spawnPos = _currentPlayer.transform.position
                + (direction * _config.cloneSpawnOffset)
                + Vector3.up * _config.cloneSpawnVerticalOffset;
            Quaternion spawnRot = _currentPlayer.transform.rotation;

            pc.Freeze();
            GameObject clone = GameObject.Instantiate(_clonePrefab, spawnPos, spawnRot);
            _container.InjectGameObject(clone);

            var boost = new CloneBoost(clone.GetComponent<Rigidbody>(), _config);
            boost.ApplyBoost(direction);

            if (_vcam != null)
                _vcam.Follow = clone.transform;

            RemoveOldPlayer();
            _lastPlayer = _currentPlayer;
            _currentPlayer = clone;
        }

        private void RemoveOldPlayer()
        {
            if (_lastPlayer != null)
            {
                GameObject.DestroyImmediate(_lastPlayer);
                _lastPlayer = null;
            }
        }

        public void Dispose()
        {
            _subscription?.Dispose();
        }
    }
}