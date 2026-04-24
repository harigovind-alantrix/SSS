using Cinemachine;
using Core.Interfaces;
using Data.Configs;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Infrastructure.Factories
{
    public class PlayerFactory : IPlayerFactory
    {
        private readonly IObjectResolver _container;
        private readonly IShopService _shopService;
        private readonly PlayerConfig _config;
        private readonly CinemachineVirtualCamera _vcam;

        public PlayerFactory(
            IObjectResolver container,
            IShopService shopService,
            PlayerConfig config,
            CinemachineVirtualCamera vcam)
        {
            _container = container;
            _shopService = shopService;
            _config = config;
            _vcam = vcam;
        }

        public GameObject Create(Vector3 position, Quaternion rotation)
        {
            var prefab = _shopService.GetSelected().prefab ?? _config.playerPrefab;
            var player = _container.Instantiate(prefab, position, rotation);

            Rigidbody rb = player.GetComponent<Rigidbody>();
            if (rb != null)
                rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            
            if (_vcam != null)
                _vcam.Follow = player.transform;
            return player;
        }
    }
}