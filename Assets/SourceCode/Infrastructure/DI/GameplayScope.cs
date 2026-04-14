using Cinemachine;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using Core.Models;
using Entities.Player;
using Features.Gameplay;
using Infrastructure.Services;

namespace Infrastructure.DI
{
    public class GameplayScope : LifetimeScope
    {
        [SerializeField] private CinemachineVirtualCamera vcam;
        [SerializeField] private GameObject initialPlayer;
        [SerializeField] private GameObject clonePrefab;
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<InputService>();
            
            builder.RegisterInstance(vcam);
            builder.RegisterInstance(initialPlayer).Keyed(InjectId.Player);
            builder.RegisterInstance(clonePrefab).Keyed(InjectId.CubePrefab);
            
            builder.RegisterEntryPoint<CloneManager>().AsSelf();
            
            builder.RegisterComponentInHierarchy<CameraZoomController>();
            builder.RegisterComponentInHierarchy<PlayerController>();
        }
    }
}