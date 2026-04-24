using Cinemachine;
using Core.Interfaces;
using Core.Messages.Gameplay;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using Core.Models;
using Features.Gameplay;
using Infrastructure.Services;
using Data.Configs;
using Features.Gameplay.Coin;
using Infrastructure.Factories;

namespace Infrastructure.DI
{
    public class GameplayScope : LifetimeScope
    {
        [SerializeField]
        private CinemachineVirtualCamera vcam;

        [SerializeField]
        private PlayerConfig playerConfig;

        [SerializeField]
        private Transform spawnPoint;

        [Header("Coins")]
        [SerializeField]
        private CoinView coinPrefab;

        [SerializeField]
        private Transform coinPoolParent;

        protected override void Configure(IContainerBuilder builder)
        {
            //! Entry points
            builder.RegisterEntryPoint<InputService>();
            builder.RegisterEntryPoint<SpawnManager>();
            builder.RegisterEntryPoint<GameOverHandler>(Lifetime.Scoped).AsImplementedInterfaces();

            builder.RegisterInstance(vcam);
            builder.RegisterInstance(playerConfig);
            builder.RegisterInstance(spawnPoint).Keyed(InjectId.SpawnPoint);

            //! Player
            builder.Register<PlayerFactory>(Lifetime.Scoped).As<IPlayerFactory>();

            //! Coins
            builder.RegisterInstance(coinPrefab).Keyed(InjectId.CoinView);
            builder.RegisterInstance(coinPoolParent).Keyed(InjectId.CoinPoolParent);
            builder.RegisterEntryPoint<CoinSystem>(Lifetime.Scoped).AsSelf();

            //! Scene components
            builder.RegisterComponentInHierarchy<CameraZoomController>();
            builder.RegisterComponentInHierarchy<CoinSpawner>();
            builder.RegisterComponentInHierarchy<PlatformPool>();
            builder.RegisterComponentInHierarchy<PlatformGenerator>();
        }
    }
}