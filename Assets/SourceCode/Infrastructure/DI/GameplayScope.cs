using Cinemachine;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using Core.Models;
using Features.Gameplay;
using Infrastructure.Services;
using Data.Configs;

namespace Infrastructure.DI
{
    public class GameplayScope : LifetimeScope
    {
        [SerializeField] private CinemachineVirtualCamera vcam;
        [SerializeField] private PlayerConfig playerConfig;
        [SerializeField] private Transform spawnPoint;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<InputService>();
            builder.RegisterEntryPoint<SpawnManager>();
            
            builder.RegisterInstance(vcam);
            builder.RegisterInstance(playerConfig);
            builder.RegisterInstance(spawnPoint).Keyed(InjectId.SpawnPoint);
            
            builder.RegisterComponentInHierarchy<CameraZoomController>();
        }
    }
}