using System;
using UnityEngine;
using Cinemachine;
using Core.Messages.Gameplay;
using Data.Configs;
using MessagePipe;
using VContainer;

namespace Features.Gameplay
{
    public class CameraZoomController : MonoBehaviour,IDisposable
    {
        public CinemachineVirtualCamera vcam;
        
        private PlayerConfig _config;
        private float targetFOV;
        private IDisposable _subscription;

        [Inject]
        public void Construct(PlayerConfig config, ISubscriber<CameraZoomEvent> zoomSubscriber)
        {
            _config       = config;
            _subscription = zoomSubscriber.Subscribe(OnZoomEvent);
        }

        void Start()
        {
            targetFOV = vcam.m_Lens.FieldOfView;
        }

        void Update()
        {
            float current = vcam.m_Lens.FieldOfView;

            vcam.m_Lens.FieldOfView = Mathf.Lerp(
                current,
                targetFOV,
                _config.zoomSpeed * Time.deltaTime
            );
        }

        private void OnZoomEvent(CameraZoomEvent evt)
        {
            targetFOV = evt.ZoomIn ? 60f : 48f;
        }

        public void Dispose()
        {
            _subscription?.Dispose();
        }
    }
}