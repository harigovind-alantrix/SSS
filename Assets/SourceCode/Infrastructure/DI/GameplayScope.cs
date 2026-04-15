using Cinemachine;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using Core.Models;
using Entities.Player;
using Features.Gameplay;
using Infrastructure.Services;
using Data.Configs;

namespace Infrastructure.DI
{
    public class GameplayScope : LifetimeScope
    {
        [SerializeField] private CinemachineVirtualCamera vcam;
        [SerializeField] private GameObject initialPlayer;
        [SerializeField] private GameObject clonePrefab;
        [SerializeField] PlayerConfig playerConfig;
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<InputService>();
            
            builder.RegisterInstance(vcam);
            builder.RegisterInstance(initialPlayer).Keyed(InjectId.Player);
            builder.RegisterInstance(clonePrefab).Keyed(InjectId.CubePrefab);
            builder.RegisterInstance(playerConfig);
            
            builder.RegisterEntryPoint<CloneManager>().AsSelf();
            
            builder.RegisterComponentInHierarchy<CameraZoomController>();
            builder.RegisterComponentInHierarchy<PlayerController>();
        }
    }
}